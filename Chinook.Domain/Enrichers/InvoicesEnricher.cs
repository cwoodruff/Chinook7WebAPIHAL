using Chinook.Domain.Helpers;

namespace Chinook.Domain.Enrichers;

public class InvoicesEnricher : ListEnricher<List<InvoiceApiModel>>
{
    private readonly InvoiceEnricher _enricher;

    public InvoicesEnricher(InvoiceEnricher enricher)
    {
        _enricher = enricher;
    }

    public override async Task Process(List<object> representations)
    {
        foreach (var invoice in representations)
        {
            await _enricher.Process(invoice as InvoiceApiModel);
        }
    }
}