using Chinook.Domain.ApiModels;
using Chinook.Domain.Helpers;

namespace Chinook.Domain.Enrichers;

public class CustomersEnricher : ListEnricher<List<CustomerApiModel>>
{
    private readonly CustomerEnricher _enricher;

    public CustomersEnricher(CustomerEnricher enricher)
    {
        _enricher = enricher;
    }

    public override async Task Process(List<object> representations)
    {
        foreach (var customer in representations)
        {
            await _enricher.Process(customer as CustomerApiModel);
        }
    }
}