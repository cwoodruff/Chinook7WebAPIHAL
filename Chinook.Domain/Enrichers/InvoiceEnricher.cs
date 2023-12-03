using Chinook.Domain.ApiModels;
using Chinook.Domain.Helpers;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;

namespace Chinook.Domain.Enrichers;

public class InvoiceEnricher(IHttpContextAccessor accessor, LinkGenerator linkGenerator) : Enricher<InvoiceApiModel>
{
    public override Task Process(InvoiceApiModel? representation)
    {
        var httpContext = accessor.HttpContext;

        var url = linkGenerator.GetUriByName(
            httpContext!,
            "GetInvoiceById",
            new { id = representation.Id },
            scheme: "https"
        );
        
        representation.AddLink(new Link
        {
            Rel = representation.Id.ToString(),
            Title = $"Invoice: #{representation.Id}",
            Href = url!
        });
        
        // enrich invoice lines
        foreach (var invoiceLine in representation.InvoiceLines)
        {
            var urlInvoiceLine = linkGenerator.GetUriByName(
                httpContext,
                "GetInvoiceLineById",
                new { id = invoiceLine.Id },
                scheme: "https"
            );

            invoiceLine.AddLink(new Link
            {
                Rel = invoiceLine.Id.ToString(),
                Title = $"Invoice Line: #{invoiceLine.Id}",
                Href = urlInvoiceLine
            });
        }
        
        // enrich customer
        var urlCustomer = linkGenerator.GetUriByName(
            httpContext,
            "GetCustomerById",
            new { id = representation.Customer.Id },
            scheme: "https"
        );

        representation.Customer.AddLink(new Link
        {
            Rel = representation.Customer.Id.ToString(),
            Title = $"Customer: #{representation.Customer.Id}",
            Href = urlCustomer
        });

        return Task.CompletedTask;
    }
}