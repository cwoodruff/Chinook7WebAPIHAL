using Chinook.Domain.ApiModels;
using Chinook.Domain.Helpers;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;

namespace Chinook.Domain.Enrichers;

public class CustomerEnricher(IHttpContextAccessor accessor, LinkGenerator linkGenerator) : Enricher<CustomerApiModel>
{
    public override Task Process(CustomerApiModel? representation)
    {
        var httpContext = accessor.HttpContext;

        var url = linkGenerator.GetUriByName(
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
        
        // enrich Support Rep
        var urlSupportRep = linkGenerator.GetUriByName(
            httpContext,
            "GetEmployeeById",
            new { id = representation.SupportRepId },
            scheme: "https"
        );

        representation.SupportRep.AddLink(new Link
        {
            Rel = representation.SupportRepId.ToString(),
            Title = $"Employee: #{representation.SupportRepId}",
            Href = urlSupportRep
        });
        
        // enrich invoices
        foreach (var invoice in representation.Invoices)
        {
            var urlInvoice = linkGenerator.GetUriByName(
                httpContext!,
                "GetInvoiceById",
                new { id = invoice.Id },
                scheme: "https"
            );
        
            invoice.AddLink(new Link
            {
                Rel = invoice.Id.ToString(),
                Title = $"Invoice: #{invoice.Id}",
                Href = urlInvoice!
            });
        }

        return Task.CompletedTask;
    }
}