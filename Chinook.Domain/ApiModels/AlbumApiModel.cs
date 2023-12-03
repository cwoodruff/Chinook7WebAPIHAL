namespace Chinook.Domain.ApiModels;

public class AlbumApiModel : BaseApiModel
{
    public string Title { get; set; }
    public int ArtistId { get; set; }

    public ArtistApiModel Artist { get; set; }

    public IList<TrackApiModel> Tracks { get; set; }
}