using Chinook.Domain.ApiModels;
using Chinook.Domain.Entities;
using Chinook.Domain.Extensions;
using FluentValidation;
using Microsoft.Extensions.Caching.Memory;

namespace Chinook.Domain.Supervisor;

public partial class ChinookSupervisor
{
    public List<ArtistApiModel> GetAllArtist()
    {
        List<Artist> artists = _artistRepository.GetAll();
        var artistApiModels = artists.ConvertAll();

        foreach (var artist in artistApiModels)
        {
            var cacheEntryOptions =
                new MemoryCacheEntryOptions().SetSlidingExpiration(TimeSpan.FromSeconds(604800))
                    .AbsoluteExpirationRelativeToNow = TimeSpan.FromSeconds(604800);
            ;
            _cache.Set(string.Concat("Artist-", artist.Id), artist, (TimeSpan)cacheEntryOptions);
        }

        return artistApiModels;
    }

    public ArtistApiModel GetArtistById(int id)
    {
        var artistApiModelCached = _cache.Get<ArtistApiModel>(string.Concat("Artist-", id));

        if (artistApiModelCached != null)
        {
            return artistApiModelCached;
        }
        else
        {
            var artist = _artistRepository.GetById(id);
            var artistApiModel = artist.Convert();
            artistApiModel.Albums = (_albumRepository.GetByArtistId(artist.Id)).ConvertAll().ToList();

            var cacheEntryOptions =
                new MemoryCacheEntryOptions().SetSlidingExpiration(TimeSpan.FromSeconds(604800))
                    .AbsoluteExpirationRelativeToNow = TimeSpan.FromSeconds(604800);
            ;
            _cache.Set(string.Concat("Artist-", artistApiModel.Id), artistApiModel, (TimeSpan)cacheEntryOptions);

            return artistApiModel;
        }
    }

    public ArtistApiModel AddArtist(ArtistApiModel newArtistApiModel)
    {
        _artistValidator.ValidateAndThrowAsync(newArtistApiModel);

        var artist = newArtistApiModel.Convert();

        artist = _artistRepository.Add(artist);
        newArtistApiModel.Id = artist.Id;
        return newArtistApiModel;
    }

    public bool UpdateArtist(ArtistApiModel artistApiModel)
    {
        _artistValidator.ValidateAndThrowAsync(artistApiModel);

        var artist = _artistRepository.GetById(artistApiModel.Id);
        
        artist.Id = artistApiModel.Id;
        artist.Name = artistApiModel.Name ?? string.Empty;

        return _artistRepository.Update(artist);
    }

    public bool DeleteArtist(int id)
        => _artistRepository.Delete(id);
}