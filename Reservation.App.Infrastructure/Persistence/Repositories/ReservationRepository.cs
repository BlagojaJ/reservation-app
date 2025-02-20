using System.Linq;
using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using Reservation.App.Application.Contracts.Persistence;
using Reservation.App.Application.Features.Reservations.Queries.GetReservationListWithUserAndPayment;
using Reservation.App.Application.RequestHelpers;
using Reservation.App.Domain.Entities;
using Reservation.App.Domain.Shared.Enums;

namespace Reservation.App.Infrastructure.Persistence.Repositories;

public class ReservationRepository(ReservationDbContext dbContext)
    : BaseRepository<Reservation>(dbContext),
        IReservationRepository
{
    public async Task<(
        IReadOnlyList<Reservation> items,
        int totalItems
    )> GetReservationsWithUsersEmailsAndPaymentsPaginated(
        PaginationParameters paginationParameters,
        SearchParameters? searchParameters,
        SortParameters? sortParameters,
        FilterParameters? filterParameters
    )
    {
        var query = _dbContext.Reservations.AsQueryable();

        query = query.Include(r => r.User);

        if (
            searchParameters != null
            && !string.IsNullOrEmpty(searchParameters.QueryProperty)
            && !string.IsNullOrEmpty(searchParameters.Query)
        )
        {
            Expression<Func<Reservation, bool>>? keySelector = null;

            keySelector = searchParameters.QueryProperty switch
            {
                "User_FirstName_LastName" => r =>
                    r.User.FirstName.StartsWith(searchParameters.Query)
                    || r.User.LastName.StartsWith(searchParameters.Query),
                "ArchivedHotelName" => r => r.ArchivedHotelName.Contains(searchParameters.Query),
                _ => null,
            };

            if (keySelector != null)
            {
                query = query.Where(keySelector);
            }
        }

        if (filterParameters != null)
        {
            if (
                filterParameters.ActiveApiAtTimeOfReservation != null
                && filterParameters.ActiveApiAtTimeOfReservation != ReservationExternalApiEnum.None
            )
            {
                query = query.Where(r =>
                    r.ActiveApisAtTimeOfReservation.Contains(
                        filterParameters.ActiveApiAtTimeOfReservation.Value
                    )
                );
            }

            if (
                filterParameters.ReservationStatus != null
                && filterParameters.ReservationStatus != ReservationStatusEnum.None
            )
            {
                query = query.Where(r => r.ReservationStatus == filterParameters.ReservationStatus);
            }

            if (filterParameters.CheckInStart != null && filterParameters.CheckInEnd != null)
            {
                var createdStart = filterParameters.CheckInStart.Value.Date;
                var createdEnd = filterParameters.CheckInEnd.Value.Date.AddDays(1).AddTicks(-1);

                query = query.Where(r =>
                    r.CheckInDate >= filterParameters.CheckInStart
                    && r.CheckInDate <= filterParameters.CheckInEnd
                );
            }

            if (filterParameters.CheckOutStart != null && filterParameters.CheckOutEnd != null)
            {
                var createdStart = filterParameters.CheckOutStart.Value.Date;
                var createdEnd = filterParameters.CheckOutEnd.Value.Date.AddDays(1).AddTicks(-1);

                query = query.Where(r =>
                    r.CheckOutDate >= filterParameters.CheckOutStart
                    && r.CheckOutDate <= filterParameters.CheckOutEnd
                );
            }

            if (filterParameters.CreatedStart != null && filterParameters.CreatedEnd != null)
            {
                var createdStart = filterParameters.CreatedStart.Value.Date;
                var createdEnd = filterParameters.CreatedEnd.Value.Date.AddDays(1).AddTicks(-1);

                query = query.Where(r => r.Date >= createdStart && r.Date <= createdEnd);
            }
        }

        if (
            sortParameters != null
            && !string.IsNullOrEmpty(sortParameters.SortBy)
            && sortParameters.SortOrder != null
        )
        {
            Expression<Func<Reservation, string>>? keySelector = null;

            keySelector = sortParameters.SortBy switch
            {
                "User_FirstName" => r => r.User.Email,
                "ArchivedHotelName" => r => r.ArchivedHotelName,
                "RoomType" => r => r.RoomType,
                _ => null,
            };

            if (keySelector != null)
            {
                query =
                    sortParameters.SortOrder == SortOrder.asc
                        ? query.OrderBy(keySelector)
                        : query.OrderByDescending(keySelector);
            }

            query = ((IOrderedQueryable<Reservation>)query).ThenByDescending(o => o.ID);
        }
        else
        {
            query = query.OrderByDescending(o => o.ID);
        }

        query = query.Include(r => r.Emails);

        var totalItems = await query.CountAsync();

        query = ApplyPagination(query, paginationParameters);

        var offerList = await query.ToListAsync();

        return (offerList, totalItems);
    }

    public async Task ChangeReservationStatus(Reservation reservation, ReservationStatusEnum status)
    {
        reservation.ReservationStatus = status;

        await UpdateAsync(reservation);
    }

    public async Task ChangeReservationAgent(Reservation reservation, int agentId)
    {
        reservation.AgentId = agentId;

        await UpdateAsync(reservation);
    }

    public async Task<Reservation?> GetReservationWithPaymentsAsync(int id)
    {
        return await _dbContext
            .Reservations.Where(r => r.ID == id)
            .Include(r => r.Payments)
            .FirstOrDefaultAsync();
    }

    public async Task<bool> CheckIfReservationAndPasswordMatch(int id, string password)
    {
        return await _dbContext.Reservations.AnyAsync(r => r.ID == id && r.Password == password);
    }
}
