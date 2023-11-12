using Chinook.Domain.ApiModels;
using Chinook.Domain.Helpers;

namespace Chinook.Domain.Enrichers;

public class TracksEnricher : ListEnricher<List<TrackApiModel>>
{
    private readonly TrackEnricher _enricher;

    public TracksEnricher(TrackEnricher enricher)
    {
        _enricher = enricher;
    }

    public override async Task Process(List<object> representations)
    {
        foreach (var track in representations)
        {
            await _enricher.Process(track as TrackApiModel);
        }
    }
}