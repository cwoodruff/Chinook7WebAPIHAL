using Chinook.Domain.ApiModels;
using Chinook.Domain.Helpers;

namespace Chinook.Domain.Enrichers;

public class AlbumsEnricher : ListEnricher<List<AlbumApiModel>>
{
    private readonly AlbumEnricher _enricher;

    public AlbumsEnricher(AlbumEnricher enricher)
    {
        _enricher = enricher;
    }

    public override async Task Process(object representations)
    {
        foreach (var album in (IEnumerable<AlbumApiModel>)representations)
        {
            await _enricher.Process(album as AlbumApiModel);
        }
    }
}