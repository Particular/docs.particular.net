using Azure.Messaging.ServiceBus;
using NServiceBus;
using NServiceBus.Pipeline;
using System;
using System.Threading.Tasks;

public class AccessToNativeMessage
{
    #region access-native-incoming-message

    class DoNotAttemptMessageProcessingIfMessageIsNotLocked : Behavior<ITransportReceiveContext>
    {
        public override Task Invoke(ITransportReceiveContext context, Func<Task> next)
        {
            var lockedUntilUtc = context.Extensions.Get<ServiceBusReceivedMessage>().LockedUntil;

            if (lockedUntilUtc <= DateTime.UtcNow)
            {
                return next();
            }

            throw new Exception($"Message lock lost for MessageId {context.Message.MessageId} and it cannot be processed.");
        }
    }

    #endregion

    class AccessOutgoingNativeMessage
    {
        async Task AccessNativeOutgoingMessageFromHandler(IMessageHandlerContext context)
        {
            #region access-native-outgoing-message
            // send a command
            var sendOptions = new SendOptions();
            sendOptions.CustomizeNativeMessage(m => m.Subject = "custom-label");
            await context.Send(new MyCommand(), sendOptions);

            // publish an event
            var publishOptions = new PublishOptions();
            publishOptions.CustomizeNativeMessage(m => m.Subject = "custom-label");
            await context.Publish(new MyEvent(), publishOptions);
            #endregion
        }

        void Customize(AzureServiceBusTransport transport)
        {
            #region access-native-outgoing-message-over-transport
            transport.OutgoingNativeMessageCustomization = (operation, message) =>
            {
                // Customize the outgoing message based on the operation
            };
            #endregion
        }

        class MyCommand { }
        class MyEvent { }
    }
}