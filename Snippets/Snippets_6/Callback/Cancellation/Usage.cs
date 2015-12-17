namespace Snippets6.Callback.Cancellation
{
    using System;
    using System.Threading;
    using NServiceBus;

    class Usage
    {
        async void Simple()
        {
            IBusSession busSession = null;

            #region CancelCallback

            SendOptions sendOptions = new SendOptions();
            var cancellationToken = new CancellationTokenSource();
            sendOptions.RegisterCancellationToken(cancellationToken.Token);
            cancellationToken.CancelAfter(TimeSpan.FromSeconds(5));
            Message message = new Message();
            try
            {
                int response = await busSession.Request<int>(message, sendOptions);
            }
            catch (OperationCanceledException ex)
            {
                // Exception that is raised when the CancellationTokenSource is canceled
            }

            #endregion
        }


    }
}