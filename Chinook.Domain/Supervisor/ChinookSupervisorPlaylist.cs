using Chinook.Domain.ApiModels;
using Chinook.Domain.Entities;
using Chinook.Domain.Extensions;
using FluentValidation;
using Microsoft.Extensions.Caching.Memory;

namespace Chinook.Domain.Supervisor;

public partial class ChinookSupervisor
{
    public List<PlaylistApiModel> GetAllPlaylist()
    {
        List<Playlist> playlists = _playlistRepository.GetAll();
        var playlistApiModels = playlists.ConvertAll();

        foreach (var playList in playlistApiModels)
        {
            var cacheEntryOptions =
                new MemoryCacheEntryOptions().SetSlidingExpiration(TimeSpan.FromSeconds(604800))
                    .AbsoluteExpirationRelativeToNow = TimeSpan.FromSeconds(604800);
            ;
            _cache.Set(string.Concat("Playlist-", playList.Id), playList, (TimeSpan)cacheEntryOptions);
        }

        return playlistApiModels;
    }

    public PlaylistApiModel GetPlaylistById(int id)
    {
        var playListApiModelCached = _cache.Get<PlaylistApiModel>(string.Concat("Playlist-", id));

        if (playListApiModelCached != null)
        {
            return playListApiModelCached;
        }
        else
        {
            var playlist = _playlistRepository.GetById(id);
            var playlistApiModel = playlist.Convert();
            playlistApiModel.Tracks = (GetTrackByMediaTypeId(playlistApiModel.Id)).ToList();

            var cacheEntryOptions =
                new MemoryCacheEntryOptions().SetSlidingExpiration(TimeSpan.FromSeconds(604800))
                    .AbsoluteExpirationRelativeToNow = TimeSpan.FromSeconds(604800);
            ;
            _cache.Set(string.Concat("Playlist-", playlistApiModel.Id), playlistApiModel,
                (TimeSpan)cacheEntryOptions);

            return playlistApiModel;
        }
    }

    public PlaylistApiModel AddPlaylist(PlaylistApiModel newPlaylistApiModel)
    {
        _playlistValidator.ValidateAndThrowAsync(newPlaylistApiModel);

        var playlist = newPlaylistApiModel.Convert();

        playlist = _playlistRepository.Add(playlist);
        newPlaylistApiModel.Id = playlist.Id;
        return newPlaylistApiModel;
    }

    public bool UpdatePlaylist(PlaylistApiModel playlistApiModel)
    {
        _playlistValidator.ValidateAndThrowAsync(playlistApiModel);

        var playlist = _playlistRepository.GetById(playlistApiModel.Id);
        
        playlist.Id = playlistApiModel.Id;
        playlist.Name = playlistApiModel.Name ?? string.Empty;

        return _playlistRepository.Update(playlist);
    }

    public bool DeletePlaylist(int id)
        => _playlistRepository.Delete(id);
}