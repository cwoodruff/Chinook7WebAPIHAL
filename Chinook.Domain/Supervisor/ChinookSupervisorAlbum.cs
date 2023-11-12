using Chinook.Domain.ApiModels;
using Chinook.Domain.Entities;
using Chinook.Domain.Extensions;
using FluentValidation;
using Microsoft.Extensions.Caching.Memory;

namespace Chinook.Domain.Supervisor;

public partial class ChinookSupervisor
{
    public List<AlbumApiModel> GetAllAlbum()
    {
        List<Album> albums = _albumRepository.GetAll();
        var albumApiModels = albums.ConvertAll();

        foreach (var album in albumApiModels)
        {
            var cacheEntryOptions =
                new MemoryCacheEntryOptions().SetSlidingExpiration(TimeSpan.FromSeconds(604800))
                    .AbsoluteExpirationRelativeToNow = TimeSpan.FromSeconds(604800);
            _cache.Set(string.Concat("Album-", album.Id), album, (TimeSpan)cacheEntryOptions);
        }

        return albumApiModels;
    }

    public AlbumApiModel? GetAlbumById(int id)
    {
        var albumApiModelCached = _cache.Get<AlbumApiModel>(string.Concat("Album-", id));

        if (albumApiModelCached != null)
        {
            return albumApiModelCached;
        }
        else
        {
            var album = _albumRepository.GetById(id);
            var albumApiModel = album.Convert();
            var result = _artistRepository.GetById(album.ArtistId);
            albumApiModel.ArtistName = result.Name;
            albumApiModel.Tracks = GetTrackByAlbumId(id);

            var cacheEntryOptions =
                new MemoryCacheEntryOptions().SetSlidingExpiration(TimeSpan.FromSeconds(604800))
                    .AbsoluteExpirationRelativeToNow = TimeSpan.FromSeconds(604800);
            ;
            _cache.Set(string.Concat("Album-", albumApiModel.Id), albumApiModel, (TimeSpan)cacheEntryOptions);

            return albumApiModel;
        }
    }

    public List<AlbumApiModel> GetAlbumByArtistId(int id)
    {
        var albums = _albumRepository.GetByArtistId(id);
        return albums.ConvertAll();
    }

    public AlbumApiModel AddAlbum(AlbumApiModel newAlbumApiModel)
    {
        _albumValidator.ValidateAndThrowAsync(newAlbumApiModel);

        var album = newAlbumApiModel.Convert();

        album = _albumRepository.Add(album);
        newAlbumApiModel.Id = album.Id;
        return newAlbumApiModel;
    }

    public bool UpdateAlbum(AlbumApiModel albumApiModel)
    {
        _albumValidator.ValidateAndThrowAsync(albumApiModel);

        var album = _albumRepository.GetById(albumApiModel.Id);
        
        album.Id = albumApiModel.Id;
        album.Title = albumApiModel.Title;
        album.ArtistId = albumApiModel.ArtistId;

        return _albumRepository.Update(album);
    }

    public bool DeleteAlbum(int id)
        => _albumRepository.Delete(id);
}