using Chinook.Domain.ApiModels;
using Chinook.Domain.Helpers;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;

namespace Chinook.Domain.Enrichers;

public class PlaylistEnricher(IHttpContextAccessor accessor, LinkGenerator linkGenerator) : Enricher<PlaylistApiModel>
{
    public override Task Process(PlaylistApiModel? representation)
    {
        var httpContext = accessor.HttpContext;

        var url = linkGenerator.GetUriByName(
            httpContext!,
            "GetPlaylistById",
            new { id = representation.Id },
            scheme: "https"
        );
        
        representation.AddLink(new Link
        {
            Rel = representation.Id.ToString(),
            Title = $"Playlist: #{representation.Id}",
            Href = url!
        });
        
        // enrich tracks
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