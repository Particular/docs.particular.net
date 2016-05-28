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

            var sendOptions = new SendOptions();
            var cancellationToken = new CancellationTokenSource();
            sendOptions.RegisterCancellationToken(cancellationToken.Token);
            cancellationToken.CancelAfter(TimeSpan.FromSeconds(5));
            var message = new Message();
            try
            {
                var response = await endpoint.Request<int>(message, sendOptions)
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