namespace Chinook.Domain.Entities;

public sealed class Artist
{
    public Artist()
    {
        Albums = new HashSet<Album>();
    }

    public int Id { get; set; }
    public string? Name { get; set; }

    public ICollection<Album>? Albums { get; set; }
}