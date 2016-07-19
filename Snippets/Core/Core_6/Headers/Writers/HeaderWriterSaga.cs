namespace Core6.Headers.Writers
{
    using System;
    using System.Threading;
    using System.Threading.Tasks;
    using Common;
    using NServiceBus;
    using NServiceBus.MessageMutator;
    using NUnit.Framework;
    using Operations.Msmq;

    [TestFixture]
    public class HeaderWriterSaga
    {
        static CountdownEvent CountdownEvent = new CountdownEvent(4);
        string endpointName = "HeaderWriterSagaV6";

        [SetUp]
        [TearDown]
        public void Setup()
        {
            QueueDeletion.DeleteQueuesForEndpoint(endpointName);
        }

        [Test]
        public async Task Write()
        {
            var endpointConfiguration = new EndpointConfiguration(endpointName);
            var typesToScan = TypeScanner.NestedTypes<HeaderWriterSaga>();
            endpointConfiguration.SetTypesToScan(typesToScan);
            endpointConfiguration.SendFailedMessagesTo("error");
            endpointConfiguration.EnableInstallers();
            endpointConfiguration.UsePersistence<InMemoryPersistence>();
            endpointConfiguration.RegisterComponents(c => c.ConfigureComponent<Mutator>(DependencyLifecycle.InstancePerCall));

            var endpointInstance = await Endpoint.Start(endpointConfiguration)
                .ConfigureAwait(false);
            await endpointInstance.SendLocal(new StartSaga1Message())
                .ConfigureAwait(false);
            CountdownEvent.Wait();
        }

        class StartSaga1Message :
            IMessage
        {
        }

        class SendFromSagaMessage :
            IMessage
        {
        }

        class Saga1 :
            Saga<Saga1.SagaData>,
            IAmStartedByMessages<StartSaga1Message>,
            IHandleMessages<ReplyFromSagaMessage>,
            IHandleMessages<ReplyToOriginatorFromSagaMessage>
        {
            public Task Handle(StartSaga1Message message, IMessageHandlerContext context)
            {
                return context.SendLocal(new SendFromSagaMessage());
            }

            public class SagaData :
                IContainSagaData
            {
                public Guid Id { get; set; }
                public string Originator { get; set; }
                public string OriginalMessageId { get; set; }
            }

            protected override void ConfigureHowToFindSaga(SagaPropertyMapper<SagaData> mapper)
            {
            }

            public Task Handle(ReplyFromSagaMessage message, IMessageHandlerContext context)
            {
                return Task.FromResult(0);
            }

            public Task Handle(ReplyToOriginatorFromSagaMessage message, IMessageHandlerContext context)
            {
                return Task.FromResult(0);
            }
        }

        class Saga2 :
            Saga<Saga2.SagaData>,
            IAmStartedByMessages<SendFromSagaMessage>,
            IHandleTimeouts<TimeoutFromSaga>
        {
            public async Task Handle(SendFromSagaMessage message, IMessageHandlerContext context)
            {
                var replyFromSagaMessage = new ReplyFromSagaMessage();
                await context.Reply(replyFromSagaMessage)
                    .ConfigureAwait(false);
                var replyToOriginatorFromSagaMessage = new ReplyToOriginatorFromSagaMessage();
                await ReplyToOriginator(context, replyToOriginatorFromSagaMessage)
                    .ConfigureAwait(false);
                await RequestTimeout(context, TimeSpan.FromMilliseconds(1), new TimeoutFromSaga())
                    .ConfigureAwait(false);
            }

            public class SagaData :
                IContainSagaData
            {
                public Guid Id { get; set; }
                public string Originator { get; set; }
                public string OriginalMessageId { get; set; }
            }

            protected override void ConfigureHowToFindSaga(SagaPropertyMapper<SagaData> mapper)
            {
            }


            public Task Timeout(TimeoutFromSaga state, IMessageHandlerContext context)
            {
                return Task.FromResult(0);
            }
        }

        class Mutator :
            IMutateIncomingTransportMessages
        {

            public Task MutateIncoming(MutateIncomingTransportMessageContext context)
            {
                var headers = context.Headers;
                if (context.IsMessageOfTye<SendFromSagaMessage>())
                {
                    var headerText = HeaderWriter.ToFriendlyString<HeaderWriterSaga>(headers);
                    SnippetLogger.Write(
                        text: headerText,
                        suffix: "Sending",
                        version: "6");
                    CountdownEvent.Signal();
                    return Task.FromResult(0);
                }
                if (context.IsMessageOfTye<ReplyFromSagaMessage>())
                {
                    var headerText = HeaderWriter.ToFriendlyString<HeaderWriterSaga>(headers);
                    SnippetLogger.Write(
                        text: headerText,
                        suffix: "Replying",
                        version: "6");
                    CountdownEvent.Signal();
                    return Task.FromResult(0);
                }
                if (context.IsMessageOfTye<ReplyToOriginatorFromSagaMessage>())
                {
                    var headerText = HeaderWriter.ToFriendlyString<HeaderWriterSaga>(headers);
                    SnippetLogger.Write(
                        text: headerText,
                        suffix: "ReplyingToOriginator",
                        version: "6");
                    CountdownEvent.Signal();
                    return Task.FromResult(0);
                }

                if (context.IsMessageOfTye<TimeoutFromSaga>())
                {
                    var headerText = HeaderWriter.ToFriendlyString<HeaderWriterSaga>(headers);
                    SnippetLogger.Write(
                        text: headerText,
                        suffix: "Timeout",
                        version: "6");
                    CountdownEvent.Signal();
                    return Task.FromResult(0);
                }
                return Task.FromResult(0);
            }
        }


        class ReplyToOriginatorFromSagaMessage :
            IMessage
        {
        }

        class ReplyFromSagaMessage :
            IMessage
        {
        }

        class MessageToReply :
            IMessage
        {
        }

        class TimeoutFromSaga
        {
        }

    }
}