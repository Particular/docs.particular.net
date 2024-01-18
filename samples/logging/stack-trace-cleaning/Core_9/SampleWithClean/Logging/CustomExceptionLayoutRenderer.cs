using System;
using System.Text;

#region Renderer
using NLog.Config;
using NLog.LayoutRenderers;

[ThreadAgnostic]
public class CustomExceptionLayoutRenderer :
    ExceptionLayoutRenderer
{
    protected override void AppendToString(StringBuilder stringBuilder, Exception exception)
    {
        var asyncString = exception.ToString();
        stringBuilder.Append(asyncString);
    }
}
#endregion