using Reservation.App.Application.Features.Reservations.Queries.GetReservationListWithUserAndPayment;
using Reservation.App.Application.RequestHelpers;
using Reservation.App.Domain.Entities;
using Reservation.App.Domain.Shared.Enums;

namespace Reservation.App.Application.Contracts.Persistence;

public interface IReservationRepository : IAsyncRepository<Reservation>
{
    /// <summary>
    /// Retrieves a paginated list of reservations along with users, emails and payment information.
    /// </summary>
    /// <param name="paginationParameters">Parameters for pagination.</param>
    /// <param name="searchParameters">Optional parameters for searching reservations.</param>
    /// <param name="sortParameters">Optional parameters for sorting reservations.</param>
    /// <param name="filterParameters">Optional parameters for filtering reservations.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains a tuple with a read-only list of reservations and the total number of items.</returns>
    Task<(
        IReadOnlyList<Reservation> items,
        int totalItems
    )> GetReservationsWithUsersEmailsAndPaymentsPaginated(
        PaginationParameters paginationParameters,
        SearchParameters? searchParameters,
        SortParameters? sortParameters,
        FilterParameters? filterParameters
    );

    /// <summary>
    /// Changes the status of a given reservation.
    /// </summary>
    /// <param name="reservation">The reservation to update.</param>
    /// <param name="status">The new status to set for the reservation.</param>
    /// <returns>A task that represents the asynchronous operation.</returns>
    Task ChangeReservationStatus(Reservation reservation, ReservationStatusEnum status);

    /// <summary>
    /// Changes the agent of a given reservation.
    /// </summary>
    /// <param name="reservation">The reservation to update.</param>
    /// <param name="agentId">The new agent ID to set for the reservation.</param>
    /// <returns>A task that represents the asynchronous operation.</returns>
    Task ChangeReservationAgent(Reservation reservation, int agentId);

    /// <summary>
    /// Retrieves a reservation along with its associated payments asynchronously.
    /// </summary>
    /// <param name="id">The unique identifier of the reservation.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains the reservation with its payments if found; otherwise, null.</returns>
    Task<Reservation?> GetReservationWithPaymentsAsync(int id);

    /// <summary>
    /// Checks if the provided reservation ID and password match.
    /// </summary>
    /// <param name="id">The reservation ID to check.</param>
    /// <param name="password">The password associated with the reservation.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains a boolean value indicating whether the reservation ID and password match.</returns>
    Task<bool> CheckIfReservationAndPasswordMatch(int id, string password);
}
