namespace Core8.Headers.Writers
{
    using System;
    using System.Threading;
    using System.Threading.Tasks;
    using Common;
    using NServiceBus;
    using NServiceBus.Pipeline;
    using NUnit.Framework;

    [TestFixture]
    public class HeaderWriterError
    {
        static ManualResetEvent ManualResetEvent = new ManualResetEvent(false);
        string endpointName = "HeaderWriterErrorV8";

        [Test]
        public async Task Write()
        {
            var errorIngestion = new EndpointConfiguration("error");
            errorIngestion.SetTypesToScan(TypeScanner.NestedTypes<ErrorMutator>());
            errorIngestion.EnableInstallers();
            errorIngestion.UseTransport(new LearningTransport());
            errorIngestion.Pipeline.Register(typeof(ErrorMutator),"Capture headers on failed messages");
            await Endpoint.Start(errorIngestion);

            var endpointConfiguration = new EndpointConfiguration(endpointName);
            endpointConfiguration.SendFailedMessagesTo("error");
            var typesToScan = TypeScanner.NestedTypes<HeaderWriterError>();
            endpointConfiguration.SetTypesToScan(typesToScan);
            endpointConfiguration.EnableInstallers();
            endpointConfiguration.UseTransport(new LearningTransport());
            endpointConfiguration.Pipeline.Register(typeof(Mutator),"Capture headers on sent messages");

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

        class ErrorMutator : Behavior<IIncomingPhysicalMessageContext>
        {
            public override Task Invoke(IIncomingPhysicalMessageContext context, Func<Task> next)
            {
                var headers = context.MessageHeaders;
                var headerText = HeaderWriter.ToFriendlyString<HeaderWriterError>(headers);
                headerText = BehaviorCleaner.CleanStackTrace(headerText);
                headerText = StackTraceCleaner.CleanStackTrace(headerText);
                SnippetLogger.Write(
                    text: headerText,
                    suffix: "Error");
                ManualResetEvent.Set();
                return Task.CompletedTask;
            }
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