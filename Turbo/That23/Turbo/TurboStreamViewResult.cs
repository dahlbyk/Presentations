using System.Collections.Generic;
using System.Diagnostics;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.ViewFeatures;

namespace Serious.AspNetCore.Turbo;

public class TurboStreamViewResult : PartialViewResult, ITurboStreamable
{
    public TurboStreamViewResult()
    {
        ViewName = "_TurboStream";
        ContentType = "text/vnd.turbo-stream.html";
    }

    public TurboStreamViewResult(
        IEnumerable<ITurboStreamable> streamables,
        IModelMetadataProvider metadataProvider,
        ModelStateDictionary modelState)
        : this()
    {
        ViewData = new ViewDataDictionary(metadataProvider, modelState)
        {
            Model = new TurboStream(streamables),
        };
    }

    public IEnumerable<TurboStreamElement> Elements =>
        ((TurboStream)ViewData.Model).Elements;

    public override async Task ExecuteResultAsync(ActionContext context)
    {
        Debug.Assert(context.HttpContext.Request.IsTurboRequest(),
            "TurboStream is only valid for Turbo requests");

        await base.ExecuteResultAsync(context);
    }
}
