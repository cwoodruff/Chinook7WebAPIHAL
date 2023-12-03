using Chinook.Domain.ApiModels;
using Chinook.Domain.Helpers;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;

namespace Chinook.Domain.Enrichers;

public class TrackEnricher(IHttpContextAccessor accessor, LinkGenerator linkGenerator) : Enricher<TrackApiModel>
{
    public override Task Process(TrackApiModel? representation)
    {
        var httpContext = accessor.HttpContext;

        var url = linkGenerator.GetUriByName(
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
        
        // enrich playlists
        foreach (var playlist in representation.Playlists)
        {
            var urlPlaylist = linkGenerator.GetUriByName(
                httpContext,
                "GetPlaylistById",
                new { id = playlist.Id },
                scheme: "https"
            );

            playlist.AddLink(new Link
            {
                Rel = playlist.Id.ToString(),
                Title = $"Playlist: #{playlist.Id}",
                Href = urlPlaylist
            });
        }
        
        // enrich album
        var urlAlbum = linkGenerator.GetUriByName(
            httpContext,
            "GetAlbumById",
            new { id = representation.AlbumId },
            scheme: "https"
        );

        representation.Album.AddLink(new Link
        {
            Rel = representation.AlbumId.ToString(),
            Title = $"Album: #{representation.AlbumId}",
            Href = urlAlbum
        });
        
        // enrich mediatype
        var urlMediaType = linkGenerator.GetUriByName(
            httpContext,
            "GetMediaTypeById",
            new { id = representation.MediaTypeId },
            scheme: "https"
        );

        representation.MediaType.AddLink(new Link
        {
            Rel = representation.MediaTypeId.ToString(),
            Title = $"MediaType: #{representation.MediaTypeId}",
            Href = urlMediaType
        });
        
        // enrich genre
        var urlGenre = linkGenerator.GetUriByName(
            httpContext,
            "GetGenreById",
            new { id = representation.GenreId },
            scheme: "https"
        );

        representation.Genre.AddLink(new Link
        {
            Rel = representation.GenreId.ToString(),
            Title = $"Genre: #{representation.GenreId}",
            Href = urlGenre
        });

        return Task.CompletedTask;
    }
}