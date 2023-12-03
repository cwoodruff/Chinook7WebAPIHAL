using Chinook.Domain.ApiModels;
using Chinook.Domain.Helpers;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;

namespace Chinook.Domain.Enrichers;

public class AlbumEnricher(IHttpContextAccessor accessor, LinkGenerator linkGenerator) : Enricher<AlbumApiModel>
{
    public override Task Process(AlbumApiModel? representation)
    {
        var httpContext = accessor.HttpContext;

        var url = linkGenerator.GetUriByName(
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

        // enrich the artist
        var urlArtist = linkGenerator.GetUriByName(
            httpContext,
            "GetArtistById",
            new { id = representation.ArtistId },
            scheme: "https"
        );

        representation.Artist.AddLink(new Link
        {
            Rel = representation.ArtistId.ToString(),
            Title = $"Artist: #{representation.ArtistId}",
            Href = urlArtist
        });

        return Task.CompletedTask;
    }
}