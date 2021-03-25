namespace ASBS_1
{
    using Microsoft.Azure.ServiceBus;
    using NServiceBus;
    using NServiceBus.Pipeline;
    using NServiceBus.Testing;
    using System;
    using System.Linq;
    using System.Threading.Tasks;
    using IMessageSession = NServiceBus.IMessageSession;

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

                throw new Exception($"Message lock lost for MessageId {context.Message.MessageId} and it cannot be processed.");
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

        class TestAccessNativeMessage
        {
            #region test-native-outgoing-message-customization-session 1.9

            [Test]
            public async Task CustomizationSession()
            {
                var testableMessageSession = new TestableMessageSession();
                var someCodeUsingTheSession = new SomeCodeUsingTheSession(testableMessageSession);

                await someCodeUsingTheSession.Execute();

                var publishedMessage = testableMessageSession.PublishedMessages.Single();
                var customization = publishedMessage.Options.GetNativeMessageCustomization();

                var nativeMessage = new Message();
                customization(nativeMessage);

                Assert.AreEqual("abc", nativeMessage.Label);
            }

            class SomeCodeUsingTheSession
            {
                readonly IMessageSession session;

                public SomeCodeUsingTheSession(IMessageSession session)
                {
                    this.session = session;
                }

                public async Task Execute()
                {
                    var options = new PublishOptions();
                    options.CustomizeNativeMessage(m => m.Label = "abc");
                    await session.Publish(new MyEvent(), options);
                }
            }

            #endregion

            #region test-native-outgoing-message-customization-handler 1.9

            [Test]
            public async Task CustomizationHandler()
            {
                var testableContext = new TestableMessageHandlerContext();

                var handler = new MyHandlerUsingCustomizations();

                await handler.Handle(new MyEvent(), testableContext);

                var publishedMessage = testableContext.PublishedMessages.Single();
                var customization = publishedMessage.Options.GetNativeMessageCustomization(testableContext);

                var nativeMessage = new Message();
                customization(nativeMessage);

                Assert.AreEqual("abc", nativeMessage.Label);
            }

            class MyHandlerUsingCustomizations : IHandleMessages<MyEvent>
            {
                public async Task Handle(MyEvent message, IMessageHandlerContext context)
                {
                    var options = new PublishOptions();
                    options.CustomizeNativeMessage(context, m => m.Label = "abc");
                    await context.Publish(message, options);
                }
            }

            #endregion

            class MyEvent { }
            class TestAttribute : Attribute { }

            static class Assert
            {
                public static void AreEqual(string expected, string result) { }
            }
        }
    }
}