namespace ASBS_1
{
    using System.Threading.Tasks;
    using NServiceBus;

    public class AccessToNativeMessage
    {
        class AccessOutgoingNativeMessage
        {
            async Task AccessNativeOutgoingMessageFromHandler(IMessageHandlerContext context)
            {
                #region access-native-outgoing-message
                // send a command
                var sendOptions = new SendOptions();
                sendOptions.CustomizeNativeMessage(m => m.Label = "custom-label");
                await context.Send(new MyCommand(), sendOptions).ConfigureAwait(false);

                // publish an event
                var publishOptions = new PublishOptions();
                publishOptions.CustomizeNativeMessage(m => m.Label = "custom-label");
                await context.Publish(new MyEvent(), publishOptions).ConfigureAwait(false);
                #endregion
            }

            class MyCommand { }
            class MyEvent { }
        }
    }
}