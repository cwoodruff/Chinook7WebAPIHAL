using Chinook.Domain.ApiModels;
using Chinook.Domain.Helpers;

namespace Chinook.Domain.Enrichers;

public class InvoiceLinesEnricher : ListEnricher<List<InvoiceLineApiModel>>
{
    private readonly InvoiceLineEnricher _enricher;

    public InvoiceLinesEnricher(InvoiceLineEnricher enricher)
    {
        _enricher = enricher;
    }

    public override async Task Process(object representations)
    {
        foreach (var invoiceline in (IEnumerable<InvoiceLineApiModel>)representations)
        {
            await _enricher.Process(invoiceline as InvoiceLineApiModel);
        }
    }
}