using Chinook.Domain.ApiModels;
using Chinook.Domain.Helpers;

namespace Chinook.Domain.Enrichers;

public class TracksEnricher(TrackEnricher enricher) : ListEnricher<List<TrackApiModel>>
{
    public override async Task Process(object representations)
    {
        foreach (var track in (IEnumerable<TrackApiModel>)representations)
        {
            await enricher.Process(track as TrackApiModel);
        }
    }
}