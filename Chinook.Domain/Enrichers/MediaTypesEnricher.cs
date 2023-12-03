using Chinook.Domain.ApiModels;
using Chinook.Domain.Helpers;

namespace Chinook.Domain.Enrichers;

public class MediaTypesEnricher(MediaTypeEnricher enricher) : ListEnricher<List<MediaTypeApiModel>>
{
    public override async Task Process(object representations)
    {
        foreach (var mediatype in (IEnumerable<MediaTypeApiModel>)representations)
        {
            await enricher.Process(mediatype as MediaTypeApiModel);
        }
    }
}