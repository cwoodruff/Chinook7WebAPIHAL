namespace Chinook.Domain.ApiModels;

public class ArtistApiModel : BaseApiModel
{
    public string? Name { get; set; }

    public IList<AlbumApiModel>? Albums { get; set; }
}