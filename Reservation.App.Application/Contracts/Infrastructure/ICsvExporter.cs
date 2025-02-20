using Reservation.App.Application.Features.Newsletters.Queries.GetNewsletterExport;

namespace Reservation.App.Application.Contracts.Infrastructure;

public interface ICsvExporter
{
    byte[] ExportNewslettersToCsv(List<NewsletterExportDto> newsletterExportDtos);
}
