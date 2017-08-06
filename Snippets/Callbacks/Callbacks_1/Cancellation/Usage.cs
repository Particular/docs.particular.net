namespace Core6.Cancellation
{
    using System;
    using System.Threading;
    using System.Threading.Tasks;
    using NServiceBus;

    class Usage
    {
        async Task Simple(IEndpointInstance endpoint)
        {
            #region CancelCallback

            var cancellationTokenSource = new CancellationTokenSource();
            cancellationTokenSource.CancelAfter(TimeSpan.FromSeconds(5));
            var message = new Message();
            try
            {
                var response = await endpoint.Request<int>(message, cancellationTokenSource.Token)
                    .ConfigureAwait(false);
            }
            catch (OperationCanceledException)
            {
                // Exception that is raised when the CancellationTokenSource is canceled
            }

            #endregion
        }

    }
}