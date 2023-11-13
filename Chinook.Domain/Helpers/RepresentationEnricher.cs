using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace Chinook.Domain.Helpers;

public class RepresentationEnricher : IAsyncResultFilter
{
    private readonly IEnumerable<IEnricher> _enrichers;
    private readonly IEnumerable<IListEnricher> _listEnrichers;

    public RepresentationEnricher(IEnumerable<IEnricher> enrichers, IEnumerable<IListEnricher> listEnrichers)
    {
        _enrichers = enrichers;
        _listEnrichers = listEnrichers;
    }
    
    public async Task OnResultExecutionAsync(ResultExecutingContext context, ResultExecutionDelegate next)
    {
        if (context.Result is ObjectResult result)
        {
            var value = result.Value;
            foreach (var enricher in _enrichers)
            {
                if (value != null && await enricher.Match(value))
                {
                    await enricher.Process(value);
                }
            }
            foreach (var listEnricher in _listEnrichers)
            {
                if (value != null && await listEnricher.Match(value))
                {
                    await listEnricher.Process(value);
                }
            }
        }
        // call this or everything is blank!
        await next();
    }
}