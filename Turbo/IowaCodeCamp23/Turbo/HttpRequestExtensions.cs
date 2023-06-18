using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Net.Http.Headers;
using Serious;

namespace Serious.AspNetCore.Turbo;

public static class HttpRequestExtensions
{
    public static bool IsTurboFrameRequest(this ViewContext viewContext, DomId id) =>
        viewContext.HttpContext.Request.IsTurboFrameRequest(id);

    public static bool IsTurboFrameRequest(this HttpRequest request, DomId id) =>
        request.Headers["turbo-frame"].Contains(id);

    public static bool IsTurboRequest(this ViewContext viewContext) => IsTurboRequest(viewContext.HttpContext.Request);

    public static bool IsTurboRequest(this HttpRequest request) =>
        request.AcceptsMediaType("text/vnd.turbo-stream.html");

    static bool AcceptsMediaType(this HttpRequest request, params string[] mediaTypes)
    {
        var mediaTypeValues = mediaTypes
            .Select(t => new MediaTypeHeaderValue(t))
            .ToList();
        var accept = request.GetTypedHeaders().Accept;
        return accept is not null && accept.Any(a => mediaTypeValues.Any(v => v.IsSubsetOf(a)));
    }
}
