using Chinook.Domain.ApiModels;
using Chinook.Domain.Helpers;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;

namespace Chinook.Domain.Enrichers;

public class EmployeeEnricher(IHttpContextAccessor accessor, LinkGenerator linkGenerator) : Enricher<EmployeeApiModel>
{
    public override Task Process(EmployeeApiModel? representation)
    {
        var httpContext = accessor.HttpContext;

        var url = linkGenerator.GetUriByName(
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
        
        // enrich ReportsTo
        var urlReportsTo = linkGenerator.GetUriByName(
            httpContext,
            "GetEmployeeById",
            new { id = representation.ReportsTo },
            scheme: "https"
        );

        representation.ReportsToNavigation.AddLink(new Link
        {
            Rel = representation.Id.ToString(),
            Title = $"Employee: #{representation.ReportsTo}",
            Href = urlReportsTo
        });
        
        // enrich DirectReports
        foreach (var directReport in representation.InverseReportsToNavigation)
        {
            var urlDirectReport = linkGenerator.GetUriByName(
                httpContext!,
                "GetEmployeeById",
                new { id = directReport.Id },
                scheme: "https"
            );
        
            directReport.AddLink(new Link
            {
                Rel = directReport.Id.ToString(),
                Title = $"Employee: #{directReport.Id}",
                Href = urlDirectReport!
            });
        }
        
        // enrich Customers
        foreach (var customer in representation.Customers)
        {
            var urlCustomer = linkGenerator.GetUriByName(
                httpContext!,
                "GetCustomerById",
                new { id = customer.Id },
                scheme: "https"
            );
        
            customer.AddLink(new Link
            {
                Rel = customer.Id.ToString(),
                Title = $"Customer: #{customer.Id}",
                Href = urlCustomer!
            });
        }
        
        return Task.CompletedTask;
    }
}