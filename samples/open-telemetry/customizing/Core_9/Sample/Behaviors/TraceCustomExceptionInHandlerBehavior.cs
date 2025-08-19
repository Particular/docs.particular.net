using NServiceBus.Pipeline;
using System;
using System.Diagnostics;
using System.Threading.Tasks;

#region add-tags-from-handler-behavior

class TraceCustomExceptionInHandlerBehavior : Behavior<IInvokeHandlerContext>
{
    public override async Task Invoke(IInvokeHandlerContext context, Func<Task> next)
    {
        try
        {
            await next();
        }
        catch (MyBusinessException e)
        {
            Activity.Current?.AddTag("sample.business-exception.reason", e.ReasonCode);
            throw;
        }
    }
}

public class MyBusinessException : Exception
{
    public int ReasonCode { get; init; }
}

#endregion