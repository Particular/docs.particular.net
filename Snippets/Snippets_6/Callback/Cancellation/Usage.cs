namespace Snippets6.Callback.Cancellation
{
    using System;
    using System.Threading;
    using NServiceBus;

    class Usage
    {
        async void Simple()
        {
            IEndpointInstance endpoint = null;

            #region CancelCallback

            SendOptions sendOptions = new SendOptions();
            CancellationTokenSource cancellationToken = new CancellationTokenSource();
            sendOptions.RegisterCancellationToken(cancellationToken.Token);
            cancellationToken.CancelAfter(TimeSpan.FromSeconds(5));
            Message message = new Message();
            try
            {
                int response = await endpoint.Request<int>(message, sendOptions);
            }
            catch (OperationCanceledException ex)
            {
                // Exception that is raised when the CancellationTokenSource is canceled
            }

            #endregion
        }


    }
}