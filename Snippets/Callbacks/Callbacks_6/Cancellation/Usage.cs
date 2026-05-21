namespace Callbacks.Cancellation
{
    using System;
    using System.Threading;
    using System.Threading.Tasks;
    using NServiceBus;

    class Usage
    {
        async Task Simple(IMessageSession messageSession)
        {
            #region CancelCallback

            var cancellationTokenSource = new CancellationTokenSource();
            cancellationTokenSource.CancelAfter(TimeSpan.FromSeconds(5));
            var message = new Message();
            try
            {
                var response = await messageSession.Request<int>(message, cancellationTokenSource.Token);
            }
            catch (OperationCanceledException)
            {
                // Exception that is raised when the CancellationTokenSource is canceled
            }

            #endregion
        }

    }
}