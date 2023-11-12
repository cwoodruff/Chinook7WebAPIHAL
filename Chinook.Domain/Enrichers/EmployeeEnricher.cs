using Chinook.Domain.ApiModels;
using Chinook.Domain.Helpers;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;

namespace Chinook.Domain.Enrichers;

public class EmployeeEnricher : Enricher<EmployeeApiModel>
{
    private readonly IHttpContextAccessor _accessor;
    private readonly LinkGenerator _linkGenerator;

    public EmployeeEnricher(IHttpContextAccessor accessor, LinkGenerator linkGenerator)
    {
        _accessor = accessor;
        _linkGenerator = linkGenerator;
    }
    
    public override Task Process(EmployeeApiModel? representation)
    {
        var httpContext = _accessor.HttpContext;

        var url = _linkGenerator.GetUriByName(
            httpContext!,
            "GetEmployeeById",
            new { id = representation.Id },
            scheme: "https"
        );
        
        representation.AddLink(new Link
        {
            Rel = representation.Id.ToString(),
            Title = $"Employee: #{representation.Id}",
            Href = url!
        });

        return Task.CompletedTask;
    }
}