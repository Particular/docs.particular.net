namespace Core5.Headers.Writers
{
    using System;
    using System.Collections.Generic;
    using System.Threading;
    using Common;
    using NServiceBus;
    using NServiceBus.MessageMutator;
    using NServiceBus.Saga;
    using NServiceBus.Unicast;
    using NUnit.Framework;
    using Operations.Msmq;

    [TestFixture]
    public class HeaderWriterSaga
    {
        static CountdownEvent CountdownEvent = new CountdownEvent(4);
        string endpointName = "HeaderWriterSagaV5";

        [SetUp]
        [TearDown]
        public void Setup()
        {
            QueueDeletion.DeleteQueuesForEndpoint(endpointName);
        }

        [Test]
        public void Write()
        {
            BusConfiguration busConfiguration = new BusConfiguration();
            busConfiguration.EndpointName(endpointName);
            IEnumerable<Type> typesToScan = TypeScanner.NestedTypes<HeaderWriterSaga>(typeof(ConfigErrorQueue));
            busConfiguration.TypesToScan(typesToScan);
            busConfiguration.EnableInstallers();
            busConfiguration.UsePersistence<InMemoryPersistence>();
            busConfiguration.RegisterComponents(c => c.ConfigureComponent<Mutator>(DependencyLifecycle.InstancePerCall));
            using (UnicastBus bus = (UnicastBus) Bus.Create(busConfiguration).Start())
            {
                bus.SendLocal(new StartSaga1Message());
                CountdownEvent.Wait();
            }
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
            public void Handle(StartSaga1Message message)
            {
                Bus.SendLocal(new SendFromSagaMessage());
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

            public void Handle(ReplyFromSagaMessage message)
            {
            }

            public void Handle(ReplyToOriginatorFromSagaMessage message)
            {

            }
        }

        class Saga2 : Saga<Saga2.SagaData>,
            IAmStartedByMessages<SendFromSagaMessage>,
            IHandleTimeouts<TimeoutFromSaga>
        {
            public void Handle(SendFromSagaMessage message)
            {
                Bus.Reply(new ReplyFromSagaMessage());
                ReplyToOriginator(new ReplyToOriginatorFromSagaMessage());
                RequestTimeout(TimeSpan.FromMilliseconds(1), new TimeoutFromSaga());
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

            public void Timeout(TimeoutFromSaga state)
            {
            }
        }

        class Mutator : IMutateIncomingTransportMessages
        {
            public void MutateIncoming(TransportMessage transportMessage)
            {
                if (transportMessage.IsMessageOfTye<SendFromSagaMessage>())
                {
                    string headerText = HeaderWriter.ToFriendlyString<HeaderWriterSaga>(transportMessage.Headers);
                    SnippetLogger.Write(text: headerText, suffix: "Sending", version: "5");
                    CountdownEvent.Signal();
                    return;
                }
                if (transportMessage.IsMessageOfTye<ReplyFromSagaMessage>())
                {
                    string headerText = HeaderWriter.ToFriendlyString<HeaderWriterSaga>(transportMessage.Headers);
                    SnippetLogger.Write(text: headerText, suffix: "Replying", version: "5");
                    CountdownEvent.Signal();
                    return;
                }
                if (transportMessage.IsMessageOfTye<ReplyToOriginatorFromSagaMessage>())
                {
                    string headerText = HeaderWriter.ToFriendlyString<HeaderWriterSaga>(transportMessage.Headers);
                    SnippetLogger.Write(text: headerText, suffix: "ReplyingToOriginator", version: "5");
                    CountdownEvent.Signal();
                    return;
                }

                if (transportMessage.IsMessageOfTye<TimeoutFromSaga>())
                {
                    string headerText = HeaderWriter.ToFriendlyString<HeaderWriterSaga>(transportMessage.Headers);
                    SnippetLogger.Write(text: headerText, suffix: "Timeout", version: "5");
                    CountdownEvent.Signal();
                    return;
                }
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
