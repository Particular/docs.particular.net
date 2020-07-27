namespace Core8.Recoverability
{
    using System;
    using System.Net;
    using System.Threading.Tasks;
    using NServiceBus.Pipeline;

#region dispose-large-exceptions
    class DisposeLargeExceptionsBehavior : Behavior<IIncomingPhysicalMessageContext>
    {
        public override async Task Invoke(IIncomingPhysicalMessageContext context, Func<Task> next)
        {
            try
            {
                await next().ConfigureAwait(false);
            }
            catch (WebException webException)
            {
                // dispose expensive resources:
                webException.Response?.Dispose();
                // rethrow to let recoverability handle the exception:
                throw;
            }
        }
    }
#endregion
}
