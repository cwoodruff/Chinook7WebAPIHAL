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

    public override async Task Process(object representations)
    {
        foreach (var track in (IEnumerable<TrackApiModel>)representations)
        {
            await _enricher.Process(track as TrackApiModel);
        }
    }
}