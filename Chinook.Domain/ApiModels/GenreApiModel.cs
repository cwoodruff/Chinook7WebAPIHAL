namespace Chinook.Domain.ApiModels;

public class GenreApiModel : BaseApiModel
{
    public string? Name { get; set; }

    public IList<TrackApiModel>? Tracks { get; set; }
}