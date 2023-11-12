using Chinook.Domain.ApiModels;
using Chinook.Domain.Helpers;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;

namespace Chinook.Domain.Enrichers;

public class TrackEnricher : Enricher<TrackApiModel>
{
    private readonly IHttpContextAccessor _accessor;
    private readonly LinkGenerator _linkGenerator;

    public TrackEnricher(IHttpContextAccessor accessor, LinkGenerator linkGenerator)
    {
        _accessor = accessor;
        _linkGenerator = linkGenerator;
    }
    
    public override Task Process(TrackApiModel? representation)
    {
        var httpContext = _accessor.HttpContext;

        var url = _linkGenerator.GetUriByName(
            httpContext!,
            "GetTrackById",
            new { id = representation.Id },
            scheme: "https"
        );
        
        representation.AddLink(new Link
        {
            Rel = representation.Id.ToString(),
            Title = $"Track: #{representation.Id}",
            Href = url!
        });

        return Task.CompletedTask;
    }
}