using Chinook.Domain.Extensions;
using FluentValidation;
using Microsoft.Extensions.Caching.Distributed;
using System.Text.Json;
using Chinook.Domain.ApiModels;
using Chinook.Domain.Entities;

namespace Chinook.Domain.Supervisor;

public partial class ChinookSupervisor
{
    public List<TrackApiModel> GetAllTrack()
    {
        List<Track> tracks = _trackRepository.GetAll();
        var trackApiModels = tracks.ConvertAll();

        foreach (var track in trackApiModels)
        {
            DistributedCacheEntryOptions cacheEntryOptions = new DistributedCacheEntryOptions();
            cacheEntryOptions.SetSlidingExpiration(TimeSpan.FromSeconds(3600));
            cacheEntryOptions.AbsoluteExpirationRelativeToNow = TimeSpan.FromSeconds(86400);

            _distributedCache.SetStringAsync($"Track-{track.Id}", JsonSerializer.Serialize(track),
                cacheEntryOptions);
        }

        return trackApiModels;
    }

    public TrackApiModel GetTrackById(int id)
    {
        var trackApiModelCached = _distributedCache.GetString($"Track-{id}");

        if (trackApiModelCached != null)
        {
            return JsonSerializer.Deserialize<TrackApiModel>(trackApiModelCached);
        }
        else
        {
            var track = _trackRepository.GetById(id);
            var trackApiModel = track.Convert();
            trackApiModel.Genre = GetGenreById(trackApiModel.GenreId);
            trackApiModel.Album = GetAlbumById(trackApiModel.AlbumId);
            trackApiModel.MediaType = GetMediaTypeById(trackApiModel.MediaTypeId);
            if (trackApiModel.Album != null) trackApiModel.AlbumName = trackApiModel.Album.Title;

            if (trackApiModel.MediaType != null) trackApiModel.MediaTypeName = trackApiModel.MediaType.Name;
            if (trackApiModel.Genre != null) trackApiModel.GenreName = trackApiModel.Genre.Name;

            DistributedCacheEntryOptions cacheEntryOptions = new DistributedCacheEntryOptions();
            cacheEntryOptions.SetSlidingExpiration(TimeSpan.FromSeconds(3600));
            cacheEntryOptions.AbsoluteExpirationRelativeToNow = TimeSpan.FromSeconds(86400);

            _distributedCache.SetStringAsync($"Track-{track.Id}", JsonSerializer.Serialize(trackApiModel),
                cacheEntryOptions);

            return trackApiModel;
        }
    }

    public List<TrackApiModel>? GetTrackByAlbumId(int id)
    {
        var tracks = _trackRepository.GetByAlbumId(id);
        if (tracks == null)
            return null;
        else
            return tracks.ConvertAll();
    }

    public List<TrackApiModel> GetTrackByGenreId(int id)
    {
        var tracks = _trackRepository.GetByGenreId(id);
        return tracks.ConvertAll();
    }

    public List<TrackApiModel> GetTrackByMediaTypeId(int id)
    {
        var tracks = _trackRepository.GetByMediaTypeId(id);
        return tracks.ConvertAll();
    }

    public List<TrackApiModel> GetTrackByPlaylistId(int id)
    {
        var tracks = _trackRepository.GetByPlaylistId(id);
        return tracks.ConvertAll();
    }

    public TrackApiModel AddTrack(TrackApiModel newTrackApiModel)
    {
        _trackValidator.ValidateAndThrowAsync(newTrackApiModel);

        var track = newTrackApiModel.Convert();

        _trackRepository.Add(track);
        newTrackApiModel.Id = track.Id;
        return newTrackApiModel;
    }

    public bool UpdateTrack(TrackApiModel trackApiModel)
    {
        _trackValidator.ValidateAndThrowAsync(trackApiModel);

        var track = _trackRepository.GetById(trackApiModel.Id);
        
        track.Id = trackApiModel.Id;
        track.Name = trackApiModel.Name;
        track.AlbumId = trackApiModel.AlbumId;
        track.MediaTypeId = trackApiModel.MediaTypeId;
        track.GenreId = trackApiModel.GenreId;
        track.Composer = trackApiModel.Composer ?? string.Empty;
        track.Milliseconds = trackApiModel.Milliseconds;
        track.Bytes = trackApiModel.Bytes;
        track.UnitPrice = trackApiModel.UnitPrice;

        return _trackRepository.Update(track);
    }

    public bool DeleteTrack(int id)
        => _trackRepository.Delete(id);

    public List<TrackApiModel> GetTrackByArtistId(int id)
    {
        var tracks = _trackRepository.GetByArtistId(id);
        return tracks.ConvertAll();
    }

    public List<TrackApiModel> GetTrackByInvoiceId(int id)
    {
        var tracks = _trackRepository.GetByInvoiceId(id);
        return tracks.ConvertAll();
    }
}