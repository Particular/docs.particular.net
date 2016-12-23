namespace Core6.Cancellation
{
    using System;
    using System.Threading;
    using System.Threading.Tasks;
    using Core6.Enum;
    using Core6.Object;
    using NServiceBus;
    using NServiceBus.Callbacks.Testing;

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

            #region CancelCallbackTesting

            var tokenSource = new CancellationTokenSource();

            var request = new Message();
            
            var session = new TestableCallbackAwareSession();
            session.When((Message msg) =>
            {
                if (msg == request)
                {
                    // When request matches, cancel the token source
                    tokenSource.Cancel();
                }
                return false;
            }, 42);

            Assert.ThrowsAsync<OperationCanceledException>(async () => await session.Request<int>(message, cancellationTokenSource.Token).ConfigureAwait(false));

            #endregion
        }

    }
}