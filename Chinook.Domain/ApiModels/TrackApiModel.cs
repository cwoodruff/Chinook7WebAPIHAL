namespace Chinook.Domain.ApiModels;

public sealed class TrackApiModel : BaseApiModel
{
    public string? Name { get; set; }
    public int AlbumId { get; set; }
    public int MediaTypeId { get; set; }
    public int GenreId { get; set; }
    public string? Composer { get; set; }
    public int Milliseconds { get; set; }
    public int Bytes { get; set; }
    public decimal UnitPrice { get; set; }
    public IList<InvoiceLineApiModel>? InvoiceLines { get; set; }
    public IList<PlaylistApiModel>? Playlists { get; set; }
    public AlbumApiModel? Album { get; set; }
    public GenreApiModel? Genre { get; set; }
    public MediaTypeApiModel? MediaType { get; set; }
}