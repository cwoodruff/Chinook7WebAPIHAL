using Chinook.Domain.ApiModels;
using Chinook.Domain.Helpers;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;

namespace Chinook.Domain.Enrichers;

public class InvoiceLineEnricher(IHttpContextAccessor accessor, LinkGenerator linkGenerator) : Enricher<InvoiceLineApiModel>
{
    public override Task Process(InvoiceLineApiModel? representation)
    {
        var httpContext = accessor.HttpContext;

        var url = linkGenerator.GetUriByName(
            httpContext!,
            "GetInvoiceLineById",
            new { id = representation.Id },
            scheme: "https"
        );
        
        representation.AddLink(new Link
        {
            Rel = representation.Id.ToString(),
            Title = $"InvoiceLine: #{representation.Id}",
            Href = url!
        });
        
        // enrich invoice
        var urlInvoice = linkGenerator.GetUriByName(
            httpContext,
            "GetInvoiceById",
            new { id = representation.InvoiceId },
            scheme: "https"
        );

        representation.Invoice.AddLink(new Link
        {
            Rel = representation.InvoiceId.ToString(),
            Title = $"Invoice: #{representation.InvoiceId}",
            Href = urlInvoice
        });
        
        //enrich track
        var urlTrack = linkGenerator.GetUriByName(
            httpContext,
            "GetTrackById",
            new { id = representation.TrackId },
            scheme: "https"
        );

        representation.Track.AddLink(new Link
        {
            Rel = representation.TrackId.ToString(),
            Title = $"Track: #{representation.TrackId}",
            Href = urlTrack
        });

        return Task.CompletedTask;
    }
}