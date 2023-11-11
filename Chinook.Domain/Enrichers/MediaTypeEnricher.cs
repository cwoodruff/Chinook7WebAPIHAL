using Chinook.Domain.Helpers;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;

namespace Chinook.Domain.Enrichers;

public class MediaTypeEnricher : Enricher<MediaTypeApiModel>
{
    private readonly IHttpContextAccessor _accessor;
    private readonly LinkGenerator _linkGenerator;

    public MediaTypeEnricher(IHttpContextAccessor accessor, LinkGenerator linkGenerator)
    {
        _accessor = accessor;
        _linkGenerator = linkGenerator;
    }
    
    public override Task Process(MediaTypeApiModel? representation)
    {
        var httpContext = _accessor.HttpContext;

        var url = _linkGenerator.GetUriByName(
            httpContext!,
            "mediatype",
            new { id = representation!.Id },
            scheme: "https"
        );
        
        representation.AddLink(new Link
        {
            Id = representation.Id.ToString(),
            Label = $"MediaType: {representation.Name} #{representation.Id}",
            Url = url!
        });

        return Task.CompletedTask;
    }
}