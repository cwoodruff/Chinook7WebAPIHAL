namespace Chinook.Domain.ApiModels;

public class MediaTypeApiModel : BaseApiModel
{
    public string? Name { get; set; }

    public IList<TrackApiModel>? Tracks { get; set; }
}