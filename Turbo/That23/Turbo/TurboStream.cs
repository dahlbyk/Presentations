using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Mvc;

namespace Serious.AspNetCore.Turbo;

// CA1711 is "Rename type name TurboStream so that it does not end in 'Stream'".
// We're not doing that. This is literally modelling Turbo Streams: https://turbo.hotwired.dev/handbook/streams
#pragma warning disable CA1711

public class TurboStream : ITurboStreamable
#pragma warning restore CA1711
{
    public TurboStream(IEnumerable<ITurboStreamable> streamables)
    {
        Elements = streamables.SelectMany(s => s.Elements);
    }

    public IEnumerable<TurboStreamElement> Elements { get; }
}

public interface ITurboStreamable
{
    IEnumerable<TurboStreamElement> Elements { get; }
}

public record TurboStreamElement(TurboStreamAction Action, DomId Target)
    : ITurboStreamable
{
    IEnumerable<TurboStreamElement> ITurboStreamable.Elements
    {
        get { yield return this; }
    }
}

public record ContentTurboStreamElement(TurboStreamAction Action, DomId Target, IHtmlContent Content)
    : TurboStreamElement(Action, Target)
{
    public ContentTurboStreamElement(TurboStreamAction action, DomId target, string html)
        : this(action, target, new HtmlString(html))
    {
    }
}

public record PartialTurboStreamElement(TurboStreamAction Action, DomId Target, PartialViewResult Partial)
    : TurboStreamElement(Action, Target);

public enum TurboStreamAction
{
    /// <summary>
    /// Appends the content within the template tag to the container designated by the target dom id.
    /// </summary>
    Append,

    /// <summary>
    /// Prepends the content within the template tag to the container designated by the target dom id.
    /// </summary>
    Prepend,

    /// <summary>
    /// Replaces the element designated by the target dom id. Note that if the content can be updated again, the
    /// new content should contain the target dom id since this replaces the entire element.
    /// </summary>
    Replace,

    /// <summary>
    /// Updates the content within the template tag to the container designated by the target dom id.
    /// </summary>
    Update,

    /// <summary>
    /// Removes the element designated by the target dom id.
    /// </summary>
    Remove,

    /// <summary>
    /// Inserts the content within the template tag before the element designated by the target dom id.
    /// </summary>
    Before,

    /// <summary>
    /// Inserts the content within the template tag after the element designated by the target dom id.
    /// </summary>
    After,

    /// <summary>
    /// Pushes a new location to the browser history.
    /// </summary>
    Location,
}
