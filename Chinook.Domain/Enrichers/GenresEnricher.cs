using Chinook.Domain.ApiModels;
using Chinook.Domain.Helpers;

namespace Chinook.Domain.Enrichers;

public class GenresEnricher(GenreEnricher enricher) : ListEnricher<List<GenreApiModel>>
{
    public override async Task Process(object representations)
    {
        foreach (var genre in (IEnumerable<GenreApiModel>)representations)
        {
            await enricher.Process(genre as GenreApiModel);
        }
    }
}