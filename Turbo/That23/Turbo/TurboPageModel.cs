using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Serious.AspNetCore.Turbo;

public class TurboPageModel : PageModel
{
    public override PageResult Page()
    {
        if (Request.IsTurboRequest() && HttpContext.Request.Method != HttpMethods.Get)
        {
            // Turbo wants a 422 response when a form submission fails validation.
            if (!ModelState.IsValid)
            {
                return new PageResult { StatusCode = StatusCodes.Status422UnprocessableEntity };
            }

            throw new UnreachableException("Don't use 'Page' method in form submissions!");
        }

        return base.Page();
    }

    /// <summary>
    /// Updates multiple parts of the page with the elements from <paramref name="streamables"/>.
    /// </summary>
    /// <param name="streamables">The parts used to update the page.</param>
    protected TurboStreamViewResult TurboStream(params ITurboStreamable[] streamables)
    {
        return new(streamables, MetadataProvider, ViewData.ModelState);
    }

    /// <summary>
    /// Replaces page URL (based on the specified route parameters) in the browser history.
    /// </summary>
    /// <param name="values">The route values to redirect to</param>
    protected TurboStreamViewResult TurboPageLocation(object? values) => TurboPageLocation(null, values);

    /// <summary>
    /// Updates the content within the template tag to the container designated by the target dom id.
    /// </summary>
    /// <param name="target">The <see cref="DomId"/> of the element where the content of the element will be replaced.</param>
    /// <param name="partial">The content to replace the target element's content with.</param>
    protected TurboStreamViewResult TurboUpdate(DomId target, PartialViewResult partial)
    {
        return TurboStream(new PartialTurboStreamElement(TurboStreamAction.Update, target, partial));
    }

    /// <summary>
    /// Updates the content within the template tag to the container designated by the target dom id.
    /// </summary>
    /// <param name="target">The <see cref="DomId"/> of the element where the content of the element will be replaced.</param>
    /// <param name="partialName">The name of the partial to replace the target element's content with.</param>
    /// <param name="model">The model to pass to the partial.</param>
    protected TurboStreamViewResult TurboUpdate(DomId target, string partialName, object model)
    {
        return TurboStream(new PartialTurboStreamElement(TurboStreamAction.Update, target, Partial(partialName, model)));
    }

    /// <summary>
    /// Updates the content within the template tag to the container designated by the target dom id.
    /// </summary>
    /// <param name="target">The <see cref="DomId"/> of the element where the content of the element will be replaced.</param>
    /// <param name="html">The encoded HTML to replace the target element's content with.</param>
    protected TurboStreamViewResult TurboUpdate(DomId target, string html)
    {
        return TurboStream(new ContentTurboStreamElement(TurboStreamAction.Update, target, html));
    }

    /// <summary>
    /// Replaces the element designated by the target dom id.
    /// </summary>
    /// <remarks>
    /// Note that if the content can be updated again, the new content should contain the target dom id since this
    /// replaces the entire element.
    /// </remarks>
    /// <param name="target">The <see cref="DomId"/> of the element to replace.</param>
    /// <param name="partial">The content to replace the target element with.</param>
    protected TurboStreamViewResult TurboReplace(DomId target, PartialViewResult partial)
    {
        return TurboStream(new PartialTurboStreamElement(TurboStreamAction.Replace, target, partial));
    }

    /// <summary>
    /// Replaces the element designated by the target dom id.
    /// </summary>
    /// <remarks>
    /// Note that if the content can be updated again, the new content should contain the target dom id since this
    /// replaces the entire element.
    /// </remarks>
    /// <param name="target">The <see cref="DomId"/> of the element to replace.</param>
    /// <param name="html">The encoded HTML to replace the target element with.</param>
    protected TurboStreamViewResult TurboReplace(DomId target, string html)
    {
        return TurboStream(new ContentTurboStreamElement(TurboStreamAction.Replace, target, html));
    }

    /// <summary>
    /// Appends the content within the template tag to the container designated by the target dom id.
    /// </summary>
    /// <param name="target">The <see cref="DomId"/> of the element where the content will be appended within.</param>
    /// <param name="partial">The content to replace the target element with.</param>
    protected TurboStreamViewResult TurboAppend(DomId target, PartialViewResult partial)
    {
        return TurboStream(new PartialTurboStreamElement(TurboStreamAction.Append, target, partial));
    }

    /// <summary>
    /// Appends the content within the template tag to the container designated by the target dom id.
    /// </summary>
    /// <param name="target">The <see cref="DomId"/> of the element where the content will be appended within.</param>
    /// <param name="html">The encoded HTML to replace the target element with.</param>
    protected TurboStreamViewResult TurboAppend(DomId target, string html)
    {
        return TurboStream(new ContentTurboStreamElement(TurboStreamAction.Append, target, html));
    }

    /// <summary>
    /// Removes the element designated by the target dom id.
    /// </summary>
    /// <param name="target">The <see cref="DomId"/> of the element to remove.</param>
    protected TurboStreamViewResult TurboRemove(DomId target)
    {
        return TurboStream(new TurboStreamElement(TurboStreamAction.Remove, target));
    }

    /// <summary>
    /// Replaces page URL (based on the specified route parameters) in the browser history.
    /// </summary>
    /// <param name="pageName">The name of the page to replace the location address with.</param>
    /// <param name="values">The route values to redirect to</param>
    protected TurboStreamViewResult TurboPageLocation(string? pageName, object? values = null)
    {
        var url = Url.Page(pageName, values) ?? "";
        return TurboStream(new ContentTurboStreamElement(TurboStreamAction.Location, new DomId("replace"), url));
    }
}
