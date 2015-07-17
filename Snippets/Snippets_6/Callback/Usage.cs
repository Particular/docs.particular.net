namespace Snippets6.Callback
{
    using System;
    using System.Threading.Tasks;
    using NServiceBus;

    public class Usage
    {
        public async Task Describe()
        {
            IBus bus = null;
            var options = new SendOptions();

            #region CallbackWithMessageAsResponse
            var responseMessage = await bus.RequestWithTransientlyHandledResponseAsync<PlaceOrderResponse>(new PlaceOrder(), options);
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
        }
    }
}