namespace CallbacksTesting1.Cancellation
{
    using System;
    using System.Threading;
    using NServiceBus;
    using NServiceBus.Callbacks.Testing;

    class Usage
    {
        void Simple()
        {
            #region CancelCallbackTesting
            
            var tokenSource = new CancellationTokenSource();

            var request = new Message();

            var session = new TestableCallbackAwareSession();
            session.When(
                matcher: (Message message) =>
                {
                    // When request matches, cancel the token source
                    if (message == request)
                    {
                        tokenSource.Cancel();
                    }
                    return false;
                },
                response: 42);

            Assert.ThrowsAsync<OperationCanceledException>(
                @delegate: async () =>
                {
                    await session.Request<int>(request, tokenSource.Token)
                        .ConfigureAwait(false);
                });

            #endregion
        }

    }
}