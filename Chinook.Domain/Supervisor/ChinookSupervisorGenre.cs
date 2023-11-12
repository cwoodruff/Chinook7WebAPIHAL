using Chinook.Domain.ApiModels;
using Chinook.Domain.Entities;
using Chinook.Domain.Extensions;
using FluentValidation;
using Microsoft.Extensions.Caching.Memory;

namespace Chinook.Domain.Supervisor;

public partial class ChinookSupervisor
{
    public List<GenreApiModel> GetAllGenre()
    {
        List<Genre> genres = _genreRepository.GetAll();
        var genreApiModels = genres.ConvertAll();

        foreach (var genre in genreApiModels)
        {
            var cacheEntryOptions =
                new MemoryCacheEntryOptions().SetSlidingExpiration(TimeSpan.FromSeconds(604800))
                    .AbsoluteExpirationRelativeToNow = TimeSpan.FromSeconds(604800);
            ;
            _cache.Set(string.Concat("Genre-", genre.Id), genre, (TimeSpan)cacheEntryOptions);
        }

        return genreApiModels;
    }

    public GenreApiModel? GetGenreById(int id)
    {
        var genreApiModelCached = _cache.Get<GenreApiModel>(string.Concat("Genre-", id));

        if (genreApiModelCached != null)
        {
            return genreApiModelCached;
        }
        else
        {
            var genre = _genreRepository.GetById(id);
            var genreApiModel = genre.Convert();
            genreApiModel.Tracks = (GetTrackByGenreId(genreApiModel.Id)).ToList();

            var cacheEntryOptions =
                new MemoryCacheEntryOptions().SetSlidingExpiration(TimeSpan.FromSeconds(604800))
                    .AbsoluteExpirationRelativeToNow = TimeSpan.FromSeconds(604800);
            ;
            _cache.Set(string.Concat("Genre-", genreApiModel.Id), genreApiModel, (TimeSpan)cacheEntryOptions);

            return genreApiModel;
        }
    }

    public GenreApiModel AddGenre(GenreApiModel newGenreApiModel)
    {
        _genreValidator.ValidateAndThrowAsync(newGenreApiModel);

        var genre = newGenreApiModel.Convert();

        genre = _genreRepository.Add(genre);
        newGenreApiModel.Id = genre.Id;
        return newGenreApiModel;
    }

    public bool UpdateGenre(GenreApiModel genreApiModel)
    {
        _genreValidator.ValidateAndThrowAsync(genreApiModel);

        var genre = _genreRepository.GetById(genreApiModel.Id);
        
        genre.Id = genreApiModel.Id;
        genre.Name = genreApiModel.Name ?? string.Empty;

        return _genreRepository.Update(genre);
    }

    public bool DeleteGenre(int id)
        => _genreRepository.Delete(id);
}