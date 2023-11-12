using Chinook.Domain.ApiModels;
using Chinook.Domain.Helpers;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;

namespace Chinook.Domain.Enrichers;

public class CustomerEnricher : Enricher<CustomerApiModel>
{
    private readonly IHttpContextAccessor _accessor;
    private readonly LinkGenerator _linkGenerator;


    public CustomerEnricher(IHttpContextAccessor accessor, LinkGenerator linkGenerator)
    {
        _accessor = accessor;
        _linkGenerator = linkGenerator;
    }
    
    public override Task Process(CustomerApiModel? representation)
    {
        var httpContext = _accessor.HttpContext;

        var url = _linkGenerator.GetUriByName(
            httpContext!,
            "GetCustomerById",
            new { id = representation.Id },
            scheme: "https"
        );
        
        representation.AddLink(new Link
        {
            Rel = representation.Id.ToString(),
            Title = $"Customer: #{representation.Id}",
            Href = url!
        });

        return Task.CompletedTask;
    }
}