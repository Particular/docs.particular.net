using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.ExceptionServices;
using Microsoft.AspNetCore.Http;

static class Extensions
{
    public static void CaptureAndThrow(this Exception exception)
    {
        var dispatchInfo = ExceptionDispatchInfo.Capture(exception);
        dispatchInfo.Throw();
    }

    public static Dictionary<string, string> RequestStringDictionary(this HttpContext context)
    {
        return context.Request.Headers.ToDictionary(x => x.Key, y => y.Value.ToString());
    }
}