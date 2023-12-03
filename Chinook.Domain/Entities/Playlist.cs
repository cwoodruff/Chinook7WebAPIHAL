namespace Chinook.Domain.Entities;

public sealed class Playlist
{
    public int Id { get; set; }
    public string? Name { get; set; }

    public ICollection<Track>? Tracks { get; set; }
}