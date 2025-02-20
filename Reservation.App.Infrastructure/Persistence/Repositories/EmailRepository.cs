using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Reservation.App.Application.Contracts.Persistence;
using Reservation.App.Domain.Entities;
using Reservation.App.Domain.Shared.Enums;

namespace Reservation.App.Infrastructure.Persistence.Repositories;

public class EmailRepository(ReservationDbContext dbContext)
    : BaseRepository<Email>(dbContext),
        IEmailRepository
{
    public async Task<bool> UpdateEmailStatus(string emailExternalId, EmailStatusEnum emailStatus)
    {
        var email = await _dbContext.Emails.FirstOrDefaultAsync(e =>
            e.ExternalEmailId == emailExternalId
        );

        if (email == null)
        {
            return false;
        }

        email.EmailStatus = emailStatus;
        await UpdateAsync(email);

        return true;
    }

    public async Task<List<string>> UpdateEmailStatuses(
        Dictionary<string, EmailStatusEnum> emailStatusUpdateDictionary
    )
    {
        var emailExternalIds = emailStatusUpdateDictionary.Keys.ToList();

        var emails = await _dbContext
            .Emails.Where(e => emailExternalIds.Contains(e.ExternalEmailId))
            .ToListAsync();

        var foundEmailIds = emails.Select(e => e.ExternalEmailId).ToHashSet();
        var notFoundEmailIds = emailExternalIds.Except(foundEmailIds).ToList();

        foreach (var email in emails)
        {
            if (emailStatusUpdateDictionary.TryGetValue(email.ExternalEmailId, out var emailStatus))
            {
                email.EmailStatus = emailStatus;
            }
        }

        await _dbContext.SaveChangesAsync();

        return notFoundEmailIds;
    }
}
