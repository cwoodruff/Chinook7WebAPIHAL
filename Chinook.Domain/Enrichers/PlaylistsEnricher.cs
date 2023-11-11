using Chinook.Domain.Helpers;

namespace Chinook.Domain.Enrichers;

public class PlaylistsEnricher : ListEnricher<List<PlaylistApiModel>>
{
    private readonly PlaylistEnricher _enricher;

    public PlaylistsEnricher(PlaylistEnricher enricher)
    {
        _enricher = enricher;
    }

    public override async Task Process(List<object> representations)
    {
        foreach (var playlist in representations)
        {
            await _enricher.Process(playlist as PlaylistApiModel);
        }
    }
}