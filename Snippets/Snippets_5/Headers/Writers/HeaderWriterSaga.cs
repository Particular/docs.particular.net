namespace Snippets5.Headers.Writers
{
    using System;
    using System.Collections.Generic;
    using System.Threading;
    using NServiceBus;
    using NServiceBus.MessageMutator;
    using NServiceBus.Saga;
    using NServiceBus.Unicast;
    using NUnit.Framework;
    using Operations.Msmq;

    [TestFixture]
    public class HeaderWriterSaga
    {
        public static CountdownEvent CountdownEvent = new CountdownEvent(3);
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
            BusConfiguration config = new BusConfiguration();
            config.EndpointName(endpointName);
            IEnumerable<Type> typesToScan = TypeScanner.NestedTypes<HeaderWriterSaga>(typeof(ConfigErrorQueue));
            config.TypesToScan(typesToScan);
            config.EnableInstallers();
            config.UsePersistence<InMemoryPersistence>();
            config.RegisterComponents(c => c.ConfigureComponent<Mutator>(DependencyLifecycle.InstancePerCall));
            using (var bus = (UnicastBus) Bus.Create(config).Start())
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
            IHandleMessages<ReplyFromSagaMessage>
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
        }

        class Saga2 : Saga<Saga2.SagaData>, 
            IAmStartedByMessages<SendFromSagaMessage>,
            IHandleTimeouts<TimeoutFromSaga>
        {
            public void Handle(SendFromSagaMessage message)
            {
                Bus.Reply(new ReplyFromSagaMessage());
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
                    SnippetLogger.Write(text: headerText, suffix: "Sending");
                    CountdownEvent.Signal();
                    return;
                }
                if (transportMessage.IsMessageOfTye<ReplyFromSagaMessage>())
                {
                    string headerText = HeaderWriter.ToFriendlyString<HeaderWriterSaga>(transportMessage.Headers);
                    SnippetLogger.Write(text: headerText, suffix: "Replying");
                    CountdownEvent.Signal();
                    return;
                }
                if (transportMessage.IsMessageOfTye<TimeoutFromSaga>())
                {
                    string headerText = HeaderWriter.ToFriendlyString<HeaderWriterSaga>(transportMessage.Headers);
                    SnippetLogger.Write(text: headerText, suffix: "Timeout");
                    CountdownEvent.Signal();
                    return;
                }
            }
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
