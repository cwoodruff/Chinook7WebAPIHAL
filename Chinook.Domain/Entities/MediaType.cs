namespace Chinook.Domain.Entities;

public sealed class MediaType
{
    public MediaType()
    {
        Tracks = new HashSet<Track>();
    }

    public int Id { get; set; }
    public string? Name { get; set; }

    public ICollection<Track>? Tracks { get; set; }
}