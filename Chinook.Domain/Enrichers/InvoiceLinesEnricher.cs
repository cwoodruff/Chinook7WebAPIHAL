using Chinook.Domain.ApiModels;
using Chinook.Domain.Helpers;

namespace Chinook.Domain.Enrichers;

public class InvoiceLinesEnricher(InvoiceLineEnricher enricher) : ListEnricher<List<InvoiceLineApiModel>>
{
    public override async Task Process(object representations)
    {
        foreach (var invoiceline in (IEnumerable<InvoiceLineApiModel>)representations)
        {
            await enricher.Process(invoiceline as InvoiceLineApiModel);
        }
    }
}