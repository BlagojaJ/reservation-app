using System.Linq.Expressions;
using Reservation.App.Application.RequestHelpers;

namespace Reservation.App.Application.Contracts.Persistence;

public interface IAsyncRepository<T>
    where T : class
{
    Task<IReadOnlyList<T>> ListAllAsync();
    Task<(IReadOnlyList<T> items, int totalItems)> ListAllAsync(
        PaginationParameters? paginationParameters = null,
        SearchParameters? searchParameters = null,
        SortParameters? sortParameters = null
    );
    Task<T?> GetByIdAsync(int id);
    Task<IReadOnlyList<T>> AddRangeAsync(IEnumerable<T> entities);
    Task<T> AddAsync(T entity);
    Task UpdateAsync(T entity);
    Task DeleteAsync(T entity);
    Task<int> CountAsync();
    Task<bool> ExistsAsync(Expression<Func<T, bool>> predicate);
}
