namespace Core7.Headers.Writers
{
    using System;
    using System.Threading;
    using System.Threading.Tasks;
    using Common;
    using CoreAll.Msmq.QueueDeletion;
    using NServiceBus;
    using NServiceBus.Pipeline;
    using NUnit.Framework;

    [TestFixture]
    public class HeaderWriterError
    {
        static ManualResetEvent ManualResetEvent = new ManualResetEvent(false);
        string endpointName = "HeaderWriterErrorV7";

        [SetUp]
        [TearDown]
        public void Setup()
        {
            DeleteEndpointQueues.DeleteQueuesForEndpoint(endpointName);
        }

        [Test]
        public async Task Write()
        {
            var endpointConfiguration = new EndpointConfiguration(endpointName);
            endpointConfiguration.SendFailedMessagesTo("error");
            var typesToScan = TypeScanner.NestedTypes<HeaderWriterError>();
            endpointConfiguration.SetTypesToScan(typesToScan);
            endpointConfiguration.EnableInstallers();
            endpointConfiguration.UseTransport<LearningTransport>();
            endpointConfiguration.Pipeline.Register(typeof(Mutator),"Capture headers on failed messages");

            var recoverability = endpointConfiguration.Recoverability();
            recoverability.Immediate(settings => settings.NumberOfRetries(1));
            recoverability.Delayed(settings =>
            {
                settings.NumberOfRetries(0);
            });

            var endpointInstance = await Endpoint.Start(endpointConfiguration)
                .ConfigureAwait(false);

            await endpointInstance.SendLocal(new MessageToSend())
                .ConfigureAwait(false);
            ManualResetEvent.WaitOne();
        }

        class MessageToSend :
            IMessage
        {
        }


        class MessageHandler :
            IHandleMessages<MessageToSend>
        {

            public Task Handle(MessageToSend message, IMessageHandlerContext context)
            {
                throw new Exception("The exception message from the handler.");
            }
        }

        class Mutator : Behavior<IIncomingLogicalMessageContext>
        {
            public Mutator(Notifications busNotifications)
            {
                var errorsNotifications = busNotifications.Errors;
                errorsNotifications.MessageSentToErrorQueue += (sender, retry) =>
                {
                    var headers = retry.Headers;
                    var headerText = HeaderWriter.ToFriendlyString<HeaderWriterError>(headers);
                    headerText = BehaviorCleaner.CleanStackTrace(headerText);
                    headerText = StackTraceCleaner.CleanStackTrace(headerText);
                    SnippetLogger.Write(
                        text: headerText,
                        suffix: "Error");
                    ManualResetEvent.Set();
                };
            }

            static bool hasCapturedMessage;

            public override Task Invoke(IIncomingLogicalMessageContext context, Func<Task> next)
            {
                if (!hasCapturedMessage && context.Message.Instance is MessageToSend)
                {
                    hasCapturedMessage = true;
                    var headers = context.Headers;
                    var sendingText = HeaderWriter.ToFriendlyString<HeaderWriterError>(headers);
                    SnippetLogger.Write(
                        text: sendingText,
                        suffix: "Sending");
                }
                return next();
            }
        }
    }
}