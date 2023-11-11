using Chinook.Domain.Helpers;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;

namespace Chinook.Domain.Enrichers;

public class InvoiceLineEnricher : Enricher<InvoiceLineApiModel>
{
    private readonly IHttpContextAccessor _accessor;
    private readonly LinkGenerator _linkGenerator;

    public InvoiceLineEnricher(IHttpContextAccessor accessor, LinkGenerator linkGenerator)
    {
        _accessor = accessor;
        _linkGenerator = linkGenerator;
    }
    
    public override Task Process(InvoiceLineApiModel? representation)
    {
        var httpContext = _accessor.HttpContext;

        var url = _linkGenerator.GetUriByName(
            httpContext!,
            "invoiceline",
            new { id = representation!.Id },
            scheme: "https"
        );
        
        representation.AddLink(new Link
        {
            Id = representation.Id.ToString(),
            Label = $"InvoiceLine: {representation.Id} #{representation.Id}",
            Url = url!
        });

        return Task.CompletedTask;
    }
}