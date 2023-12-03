using Chinook.Domain.ApiModels;
using Chinook.Domain.Helpers;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;

namespace Chinook.Domain.Enrichers;

public class ArtistEnricher(IHttpContextAccessor accessor, LinkGenerator linkGenerator) : Enricher<ArtistApiModel>
{
    public override Task Process(ArtistApiModel? representation)
    {
        var httpContext = accessor.HttpContext;

        var url = linkGenerator.GetUriByName(
            httpContext!,
            "GetArtistById",
            new { id = representation.Id },
            scheme: "https"
        );
        
        representation.AddLink(new Link
        {
            Rel = representation.Id.ToString(),
            Title = $"Artist: #{representation.Id}",
            Href = url!
        });
        
        // enrich the albums
        foreach (var album in representation.Albums)
        {
            var urlAlbum = linkGenerator.GetUriByName(
                httpContext,
                "GetAlbumById",
                new { id = representation.Id },
                scheme: "https"
            );

            album.AddLink(new Link
            {
                Rel = representation.Id.ToString(),
                Title = $"Album: #{representation.Id}",
                Href = urlAlbum
            });
        }

        return Task.CompletedTask;
    }
}