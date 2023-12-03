using Chinook.Domain.ApiModels;
using Chinook.Domain.Helpers;

namespace Chinook.Domain.Enrichers;

public class ArtistsEnricher(ArtistEnricher enricher) : ListEnricher<List<ArtistApiModel>>
{
    public override async Task Process(object representations)
    {
        foreach (var artist in (IEnumerable<ArtistApiModel>)representations)
        {
            await enricher.Process(artist as ArtistApiModel);
        }
    }
}