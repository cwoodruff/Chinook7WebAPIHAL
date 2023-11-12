using Chinook.Domain.ApiModels;
using Chinook.Domain.Helpers;

namespace Chinook.Domain.Enrichers;

public class ArtistsEnricher : ListEnricher<List<ArtistApiModel>>
{
    private readonly ArtistEnricher _enricher;

    public ArtistsEnricher(ArtistEnricher enricher)
    {
        _enricher = enricher;
    }

    public override async Task Process(List<object> representations)
    {
        foreach (var artist in representations)
        {
            await _enricher.Process(artist as ArtistApiModel);
        }
    }
}