using Chinook.Domain.ApiModels;
using Chinook.Domain.Helpers;

namespace Chinook.Domain.Enrichers;

public class CustomersEnricher(CustomerEnricher enricher) : ListEnricher<List<CustomerApiModel>>
{
    public override async Task Process(object representations)
    {
        foreach (var customer in (IEnumerable<CustomerApiModel>)representations)
        {
            await enricher.Process(customer as CustomerApiModel);
        }
    }
}