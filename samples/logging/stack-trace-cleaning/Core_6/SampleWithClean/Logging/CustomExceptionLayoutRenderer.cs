using System;
using System.Text;
#region Renderer
using AsyncFriendlyStackTrace;
using NLog.Config;
using NLog.LayoutRenderers;
[ThreadAgnostic]
public class CustomExceptionLayoutRenderer : ExceptionLayoutRenderer
{
    protected override void AppendToString(StringBuilder stringBuilder, Exception exception)
    {
        string asyncString = exception.ToAsyncString();
        stringBuilder.Append(asyncString);
    }
}
#endregion