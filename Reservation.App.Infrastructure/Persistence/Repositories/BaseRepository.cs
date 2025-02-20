using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using Reservation.App.Application.Contracts.Persistence;
using Reservation.App.Application.RequestHelpers;

namespace Reservation.App.Infrastructure.Persistence.Repositories;

public class BaseRepository<T> : IAsyncRepository<T>
    where T : class
{
    protected readonly ReservationDbContext _dbContext;

    public BaseRepository(ReservationDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<IReadOnlyList<T>> ListAllAsync()
    {
        return await _dbContext.Set<T>().ToListAsync();
    }

    public async Task<(IReadOnlyList<T> items, int totalItems)> ListAllAsync(
        PaginationParameters? paginationParameters = null,
        SearchParameters? searchParameters = null,
        SortParameters? sortParameters = null
    )
    {
        var query = _dbContext.Set<T>().AsQueryable();

        if (searchParameters != null)
        {
            query = ApplySearchCondition(query, searchParameters);
        }

        if (sortParameters != null)
        {
            query = ApplySortCondition(query, sortParameters);
        }
        else
        {
            query = ApplySortCondition(query, new() { SortBy = "ID", SortOrder = SortOrder.asc });
        }

        // Count the number of items before pagination is applied
        var totalItems = await query.CountAsync();

        if (paginationParameters != null)
        {
            query = ApplyPagination(query, paginationParameters);
        }

        var items = await query.ToListAsync();

        return (items, totalItems);
    }

    public virtual async Task<T?> GetByIdAsync(int id)
    {
        return await _dbContext.Set<T>().FindAsync(id);
    }

    public async Task<IReadOnlyList<T>> AddRangeAsync(IEnumerable<T> entities)
    {
        await _dbContext.Set<T>().AddRangeAsync(entities);
        await _dbContext.SaveChangesAsync();

        return entities.ToList();
    }

    public async Task<T> AddAsync(T entity)
    {
        try
        {
            await _dbContext.Set<T>().AddAsync(entity);
            await _dbContext.SaveChangesAsync();
        }
        catch
        {
            // If adding fails, the entity will not be tracked => Added for the purpose of RTours synchronization not to fail for following entities
            _dbContext.Entry(entity).State = EntityState.Detached;
            throw;
        }

        return entity;
    }

    public async Task UpdateAsync(T entity)
    {
        try
        {
            _dbContext.Entry(entity).State = EntityState.Modified;
            await _dbContext.SaveChangesAsync();
        }
        catch
        {
            // If updating fails, the entity will not be tracked => Added for the purpose of RTours synchronization not to fail for following entities
            _dbContext.Entry(entity).State = EntityState.Detached;
            throw;
        }
    }

    public async Task DeleteAsync(T entity)
    {
        _dbContext.Set<T>().Remove(entity);
        await _dbContext.SaveChangesAsync();
    }

    public async Task<int> CountAsync()
    {
        return await _dbContext.Set<T>().CountAsync();
    }

    protected static IQueryable<T> ApplyPagination(
        IQueryable<T> query,
        PaginationParameters paginationParameters
    )
    {
        query = query
            .Skip((paginationParameters.PageNumber - 1) * paginationParameters.PageSize)
            .Take(paginationParameters.PageSize);

        return query;
    }

    protected static IQueryable<T> ApplySearchCondition(
        IQueryable<T> query,
        SearchParameters searchParameters
    )
    {
        if (
            string.IsNullOrWhiteSpace(searchParameters.QueryProperty)
            || string.IsNullOrWhiteSpace(searchParameters.Query)
        )
        {
            return query;
        }

        // Use reflection to get the property value dynamically
        var property = typeof(T).GetProperty(searchParameters.QueryProperty);

        if (property != null)
        {
            // Build the dynamic lambda expression => Note: Expression is from LINQ
            var parameter = Expression.Parameter(typeof(T), "x");
            var propertyAccess = Expression.MakeMemberAccess(parameter, property);
            var filterExpression = Expression.Lambda<Func<T, bool>>(
                Expression.Call(
                    propertyAccess,
                    "Contains",
                    null,
                    Expression.Constant(searchParameters.Query)
                ),
                parameter
            );

            // Apply the dynamic filter to the query
            query = query.Where(filterExpression);
        }

        return query;
    }

    protected static IQueryable<T> ApplySortCondition(
        IQueryable<T> query,
        SortParameters sortParameters
    )
    {
        if (string.IsNullOrWhiteSpace(sortParameters.SortBy) || !sortParameters.SortOrder.HasValue)
        {
            return query;
        }

        var property = typeof(T).GetProperty(sortParameters.SortBy);

        // TODO: Make sorting available for properties inside Navigation properties
        if (property != null)
        {
            var parameter = Expression.Parameter(typeof(T), "x");
            var propertyAccess = Expression.MakeMemberAccess(parameter, property);

            var convertedProperty = Expression.Convert(propertyAccess, typeof(object));

            var sortExpression = Expression.Lambda<Func<T, object>>(convertedProperty, parameter);

            var isAscending = sortParameters.SortOrder == SortOrder.asc;
            query = isAscending
                ? query.OrderBy(sortExpression)
                : query.OrderByDescending(sortExpression);
        }

        return query;
    }

    public Task<bool> ExistsAsync(Expression<Func<T, bool>> predicate)
    {
        return _dbContext.Set<T>().AnyAsync(predicate);
    }
}
