namespace Snippets6.Callback
{
    using System;
    using System.Threading;
    using System.Threading.Tasks;
    using NServiceBus;

    public class Usage
    {
        public async Task Describe()
        {
            IBus bus = null;
            PlaceOrderResponse responseMessage;
            var options = new SendOptions();

            #region CallbackWithMessageAsResponse
            responseMessage = await bus.RequestWithTransientlyHandledResponseAsync<PlaceOrderResponse>(new PlaceOrder(), options);
            #endregion

            #region TriggerCallbackWithMessageAsResponse
            bus.Reply(new PlaceOrderResponse());
            #endregion

            #region CallbackWithEnumAsResponse
            var response = await bus.RequestWithTransientlyHandledResponseAsync<LegacyEnumResponse<Status>>(new PlaceOrder(), options);
            var responseEnum = response.Status;
            #endregion

            #region TriggerCallbackWithEnumAsResponse
            bus.Reply(new LegacyEnumResponse<Status>(Status.OK));
            #endregion

            #region CancelCallback
            var cs = new CancellationTokenSource();
            options.RegisterCancellationToken(cs.Token);
            try
            {
                responseMessage = await bus.RequestWithTransientlyHandledResponseAsync<PlaceOrderResponse>(new PlaceOrder(), options);
            }
            catch (OperationCanceledException ex)
            {
                // Exception that is raised when the CancellationTokenSource is canceled
            }
            #endregion
        }
    }
}