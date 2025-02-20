using Reservation.App.Domain.Entities;
using Reservation.App.Domain.Shared.Enums;

namespace Reservation.App.Application.Contracts.Persistence;

public interface IEmailRepository : IAsyncRepository<Email>
{
    /// Updates the status of an email based on the provided external email ID.
    /// </summary>
    /// <param name="emailExternalId">The external identifier of the email whose status is to be updated.</param>
    /// <param name="emailStatus">The new status to be assigned to the email.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains a boolean value indicating whether the update was successful.</returns>
    public Task<bool> UpdateEmailStatus(string emailExternalId, EmailStatusEnum emailStatus);

    /// <summary>
    /// Updates the statuses of multiple emails based on their external IDs.
    /// </summary>
    /// <param name="emailStatusUpdateDictionary">A dictionary where the key is the email external ID and the value is the new email status.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains a list of strings representing the email external IDs that were not updated.</returns>
    public Task<List<string>> UpdateEmailStatuses(
        Dictionary<string, EmailStatusEnum> emailStatusUpdateDictionary
    );
}
