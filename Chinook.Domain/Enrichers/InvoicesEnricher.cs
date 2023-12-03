using Chinook.Domain.ApiModels;
using Chinook.Domain.Helpers;

namespace Chinook.Domain.Enrichers;

public class InvoicesEnricher(InvoiceEnricher enricher) : ListEnricher<List<InvoiceApiModel>>
{
    public override async Task Process(object representations)
    {
        foreach (var invoice in (IEnumerable<InvoiceApiModel>)representations)
        {
            await enricher.Process(invoice as InvoiceApiModel);
        }
    }
}