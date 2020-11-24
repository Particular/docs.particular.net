namespace ASBS_1
{
    using System;
    using System.Threading.Tasks;
    using Microsoft.Azure.ServiceBus;
    using NServiceBus;
    using NServiceBus.Pipeline;

    public class AccessToNativeMessage
    {
        #region access-native-incoming-message 1.4

        class DoNotAttemptMessageProcessingIfMessageIsNotLocked : Behavior<ITransportReceiveContext>
        {
            public override Task Invoke(ITransportReceiveContext context, Func<Task> next)
            {
                var lockedUntilUtc = context.Extensions.Get<Message>().SystemProperties.LockedUntilUtc;

                if (lockedUntilUtc <= DateTime.UtcNow)
                {
                    return next();
                }

                context.AbortReceiveOperation();

                return Task.CompletedTask;
            }
        }

        #endregion

        class AccessOutgoingNativeMessage
        {
            async Task AccessNativeOutgoingMessageFromHandler(IMessageHandlerContext context)
            {
                #region access-native-outgoing-message-from-handler 1.7
                // send a command
                var sendOptions = new SendOptions();
                sendOptions.CustomizeNativeMessage(context, m => m.Label = "custom-label");
                await context.Send(new MyCommand(), sendOptions).ConfigureAwait(false);

                // publish an event
                var publishOptions = new PublishOptions();
                publishOptions.CustomizeNativeMessage(context, m => m.Label = "custom-label");
                await context.Publish(new MyEvent(), publishOptions).ConfigureAwait(false);
                #endregion
            }

            async Task AccessNativeOutgoingMessageWithMessageSession(IEndpointInstance messageSession)
            {
                #region access-native-outgoing-message-with-messagesession 1.7
                // send a command
                var sendOptions = new SendOptions();
                sendOptions.CustomizeNativeMessage(m => m.Label = "custom-label");
                await messageSession.Send(new MyCommand(), sendOptions).ConfigureAwait(false);

                // publish an event
                var publishOptions = new PublishOptions();
                publishOptions.CustomizeNativeMessage(m => m.Label = "custom-label");
                await messageSession.Publish(new MyEvent(), publishOptions).ConfigureAwait(false);
                #endregion
            }

            #region access-native-outgoing-message-from-physical-behavior 1.7
            public class PhysicalBehavior : Behavior<IIncomingPhysicalMessageContext>
            {
                public override async Task Invoke(IIncomingPhysicalMessageContext context, Func<Task> next)
                {
                    var sendOptions = new SendOptions();
                    sendOptions.CustomizeNativeMessage(context, m => m.Label = "custom-label");

                    await context.Send(new MyCommand(), sendOptions);

                    await next();
                }
            }
            #endregion

            #region access-native-outgoing-message-from-logical-behavior 1.7
            public class LogicalBehavior : Behavior<IIncomingLogicalMessageContext>
            {
                public override async Task Invoke(IIncomingLogicalMessageContext context, Func<Task> next)
                {
                    var publishOptions = new PublishOptions();
                    publishOptions.CustomizeNativeMessage(context, m => m.Label = "custom-label");

                    await context.Publish(new MyEvent(), publishOptions);

                    await next();
                }
            }
            #endregion

            class MyCommand { }
            class MyEvent { }
        }
    }
}