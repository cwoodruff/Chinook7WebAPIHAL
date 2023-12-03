namespace Chinook.Domain.ApiModels;

public class PlaylistApiModel : BaseApiModel
{
    public string? Name { get; set; }

    public IList<TrackApiModel>? Tracks { get; set; }
}