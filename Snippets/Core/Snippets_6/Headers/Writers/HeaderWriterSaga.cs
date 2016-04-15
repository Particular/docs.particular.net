namespace Core6.Headers.Writers
{
    using System;
    using System.Collections.Generic;
    using System.Threading;
    using System.Threading.Tasks;
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
            EndpointConfiguration endpointConfiguration = new EndpointConfiguration(endpointName);
            IEnumerable<Type> typesToScan = TypeScanner.NestedTypes<HeaderWriterSaga>();
            endpointConfiguration.SetTypesToScan(typesToScan);
            endpointConfiguration.SendFailedMessagesTo("error");
            endpointConfiguration.EnableInstallers();
            endpointConfiguration.UsePersistence<InMemoryPersistence>();
            endpointConfiguration.RegisterComponents(c => c.ConfigureComponent<Mutator>(DependencyLifecycle.InstancePerCall));

            IEndpointInstance endpoint = await Endpoint.Start(endpointConfiguration);
            await endpoint.SendLocal(new StartSaga1Message());
            CountdownEvent.Wait();
        }

        class StartSaga1Message : IMessage
        {
        }

        class SendFromSagaMessage : IMessage
        {
        }

        class Saga1 : Saga<Saga1.SagaData>,
            IAmStartedByMessages<StartSaga1Message>,
            IHandleMessages<ReplyFromSagaMessage>,
            IHandleMessages<ReplyToOriginatorFromSagaMessage>
        {
            public async Task Handle(StartSaga1Message message, IMessageHandlerContext context)
            {
                await context.SendLocal(new SendFromSagaMessage());
            }

            public class SagaData : IContainSagaData
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

        class Saga2 : Saga<Saga2.SagaData>, 
            IAmStartedByMessages<SendFromSagaMessage>,
            IHandleTimeouts<TimeoutFromSaga>
        {
            public async Task Handle(SendFromSagaMessage message, IMessageHandlerContext context)
            {
                await context.Reply(new ReplyFromSagaMessage());
                await ReplyToOriginator(context, new ReplyToOriginatorFromSagaMessage());
                await RequestTimeout(context, TimeSpan.FromMilliseconds(1), new TimeoutFromSaga());
            }

            public class SagaData : IContainSagaData
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

        class Mutator : IMutateIncomingTransportMessages
        {
        
            public Task MutateIncoming(MutateIncomingTransportMessageContext context)
            {
                if (context.IsMessageOfTye<SendFromSagaMessage>())
                {
                    string headerText = HeaderWriter.ToFriendlyString<HeaderWriterSaga>(context.Headers);
                    SnippetLogger.Write(text: headerText, suffix: "Sending", version: "6");
                    CountdownEvent.Signal();
                    return Task.FromResult(0);
                }
                if (context.IsMessageOfTye<ReplyFromSagaMessage>())
                {
                    string headerText = HeaderWriter.ToFriendlyString<HeaderWriterSaga>(context.Headers);
                    SnippetLogger.Write(text: headerText, suffix: "Replying", version: "6");
                    CountdownEvent.Signal();
                    return Task.FromResult(0);
                }
                if (context.IsMessageOfTye<ReplyToOriginatorFromSagaMessage>())
                {
                    string headerText = HeaderWriter.ToFriendlyString<HeaderWriterSaga>(context.Headers);
                    SnippetLogger.Write(text: headerText, suffix: "ReplyingToOriginator", version: "6");
                    CountdownEvent.Signal();
                    return Task.FromResult(0);
                }

                if (context.IsMessageOfTye<TimeoutFromSaga>())
                {
                    string headerText = HeaderWriter.ToFriendlyString<HeaderWriterSaga>(context.Headers);
                    SnippetLogger.Write(text: headerText, suffix: "Timeout", version: "6");
                    CountdownEvent.Signal();
                    return Task.FromResult(0);
                }
                return Task.FromResult(0);
            }
        }


        class ReplyToOriginatorFromSagaMessage : IMessage
        {
        }
        class ReplyFromSagaMessage : IMessage
        {
        }

        class MessageToReply : IMessage
        {
        }

        class TimeoutFromSaga
        {
        }

    }
}
