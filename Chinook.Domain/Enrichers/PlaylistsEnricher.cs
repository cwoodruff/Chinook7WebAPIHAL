using Chinook.Domain.ApiModels;
using Chinook.Domain.Helpers;

namespace Chinook.Domain.Enrichers;

public class PlaylistsEnricher(PlaylistEnricher enricher) : ListEnricher<List<PlaylistApiModel>>
{
    public override async Task Process(object representations)
    {
        foreach (var playlist in (IEnumerable<PlaylistApiModel>)representations)
        {
            await enricher.Process(playlist as PlaylistApiModel);
        }
    }
}