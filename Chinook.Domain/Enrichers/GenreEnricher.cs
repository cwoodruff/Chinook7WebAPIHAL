using Chinook.Domain.ApiModels;
using Chinook.Domain.Helpers;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;

namespace Chinook.Domain.Enrichers;

public class GenreEnricher(IHttpContextAccessor accessor, LinkGenerator linkGenerator) : Enricher<GenreApiModel>
{
    public override Task Process(GenreApiModel? representation)
    {
        var httpContext = accessor.HttpContext;

        var url = linkGenerator.GetUriByName(
            httpContext!,
            "GetGenreById",
            new { id = representation.Id },
            scheme: "https"
        );
        
        representation.AddLink(new Link
        {
            Rel = representation.Id.ToString(),
            Title = $"genre: #{representation.Id}",
            Href = url!
        });
        
        // enrich the tracks
        foreach (var track in representation.Tracks)
        {
            var urlTrack = linkGenerator.GetUriByName(
                httpContext,
                "GetTrackById",
                new { id = track.Id },
                scheme: "https"
            );

            track.AddLink(new Link
            {
                Rel = track.Id.ToString(),
                Title = $"Track: #{track.Id}",
                Href = urlTrack
            });
        }

        return Task.CompletedTask;
    }
}