using Chinook.Domain.ApiModels;
using Chinook.Domain.Entities;
using Chinook.Domain.Extensions;
using FluentValidation;
using Microsoft.Extensions.Caching.Memory;

namespace Chinook.Domain.Supervisor;

public partial class ChinookSupervisor
{
    public List<MediaTypeApiModel> GetAllMediaType()
    {
        List<MediaType> mediaTypes = _mediaTypeRepository.GetAll();
        var mediaTypeApiModels = mediaTypes.ConvertAll();

        foreach (var mediaType in mediaTypeApiModels)
        {
            var cacheEntryOptions =
                new MemoryCacheEntryOptions().SetSlidingExpiration(TimeSpan.FromSeconds(604800))
                    .AbsoluteExpirationRelativeToNow = TimeSpan.FromSeconds(604800);
            ;
            _cache.Set(string.Concat("MediaType-", mediaType.Id), mediaType, (TimeSpan)cacheEntryOptions);
        }

        return mediaTypeApiModels;
    }

    public MediaTypeApiModel? GetMediaTypeById(int id)
    {
        var mediaTypeApiModelCached = _cache.Get<MediaTypeApiModel>(string.Concat("MediaType-", id));

        if (mediaTypeApiModelCached != null)
        {
            return mediaTypeApiModelCached;
        }
        else
        {
            var mediaType = _mediaTypeRepository.GetById(id);
            var mediaTypeApiModel = mediaType.Convert();
            mediaTypeApiModel.Tracks = (GetTrackByMediaTypeId(mediaTypeApiModel.Id)).ToList();

            var cacheEntryOptions =
                new MemoryCacheEntryOptions().SetSlidingExpiration(TimeSpan.FromSeconds(604800))
                    .AbsoluteExpirationRelativeToNow = TimeSpan.FromSeconds(604800);
            ;
            _cache.Set(string.Concat("MediaType-", mediaTypeApiModel.Id), mediaTypeApiModel,
                (TimeSpan)cacheEntryOptions);

            return mediaTypeApiModel;
        }
    }

    public MediaTypeApiModel AddMediaType(MediaTypeApiModel newMediaTypeApiModel)
    {
        _mediaTypeValidator.ValidateAndThrowAsync(newMediaTypeApiModel);

        var mediaType = newMediaTypeApiModel.Convert();

        mediaType = _mediaTypeRepository.Add(mediaType);
        newMediaTypeApiModel.Id = mediaType.Id;
        return newMediaTypeApiModel;
    }

    public bool UpdateMediaType(MediaTypeApiModel mediaTypeApiModel)
    {
        _mediaTypeValidator.ValidateAndThrowAsync(mediaTypeApiModel);

        var mediaType = _mediaTypeRepository.GetById(mediaTypeApiModel.Id);
        
        mediaType.Id = mediaTypeApiModel.Id;
        mediaType.Name = mediaTypeApiModel.Name ?? string.Empty;

        return _mediaTypeRepository.Update(mediaType);
    }

    public bool DeleteMediaType(int id)
        => _mediaTypeRepository.Delete(id);
}