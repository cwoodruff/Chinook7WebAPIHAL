using Chinook.Domain.ApiModels;
using Chinook.Domain.Helpers;

namespace Chinook.Domain.Enrichers;

public class GenresEnricher : ListEnricher<List<GenreApiModel>>
{
    private readonly GenreEnricher _enricher;

    public GenresEnricher(GenreEnricher enricher)
    {
        _enricher = enricher;
    }

    public override async Task Process(object representations)
    {
        foreach (var genre in (IEnumerable<GenreApiModel>)representations)
        {
            await _enricher.Process(genre as GenreApiModel);
        }
    }
}