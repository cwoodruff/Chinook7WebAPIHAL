namespace Chinook.Domain.Helpers;

public class Link
{
    public string Rel { get; set; }
    public string Title { get; set; }
    public string Href { get; set; }
    
    public Link(string rel, string href, string title = null)
    {
        Rel = rel;
        Href = href;
        Title = title;
    }

    public Link()
    {
    }

    public override string ToString()
    {
        return Href;
    }
}