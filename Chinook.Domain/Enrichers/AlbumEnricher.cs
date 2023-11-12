using Chinook.Domain.ApiModels;
using Chinook.Domain.Helpers;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;

namespace Chinook.Domain.Enrichers;

public class AlbumEnricher : Enricher<AlbumApiModel>
{
    private readonly IHttpContextAccessor _accessor;
    private readonly LinkGenerator _linkGenerator;

    public AlbumEnricher(IHttpContextAccessor accessor, LinkGenerator linkGenerator)
    {
        _accessor = accessor;
        _linkGenerator = linkGenerator;
    }

    public override Task Process(AlbumApiModel? representation)
    {
        var httpContext = _accessor.HttpContext;

        var url = _linkGenerator.GetUriByName(
            httpContext,
            "GetAlbumById",
            new { id = representation.Id },
            scheme: "https"
        );
        
        representation.AddLink(new Link
            {
                Rel = representation.Id.ToString(),
                Title = $"Album: #{representation.Id}",
                Href = url
            });

        return Task.CompletedTask;
    }
}