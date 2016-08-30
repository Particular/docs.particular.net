namespace Core5.Headers.Writers
{
    using System;
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
            QueueDeletionUtils.DeleteQueue(endpointName);
        }

        [Test]
        public void Write()
        {
            var busConfiguration = new BusConfiguration();
            busConfiguration.EndpointName(endpointName);
            var typesToScan = TypeScanner.NestedTypes<HeaderWriterSaga>(typeof(ConfigErrorQueue));
            busConfiguration.TypesToScan(typesToScan);
            busConfiguration.EnableInstallers();
            busConfiguration.UsePersistence<InMemoryPersistence>();
            busConfiguration.RegisterComponents(
                registration: components =>
                {
                    components.ConfigureComponent<Mutator>(DependencyLifecycle.InstancePerCall);
                });
            using (var bus = (UnicastBus) Bus.Create(busConfiguration).Start())
            {
                var message = new StartSaga1Message();
                bus.SendLocal(message);
                CountdownEvent.Wait();
            }
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
            public void Handle(StartSaga1Message message)
            {
                var sendFromSagaMessage = new SendFromSagaMessage();
                Bus.SendLocal(sendFromSagaMessage);
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

            public void Handle(ReplyFromSagaMessage message)
            {
            }

            public void Handle(ReplyToOriginatorFromSagaMessage message)
            {

            }
        }

        class Saga2 :
            Saga<Saga2.SagaData>,
            IAmStartedByMessages<SendFromSagaMessage>,
            IHandleTimeouts<TimeoutFromSaga>
        {
            public void Handle(SendFromSagaMessage message)
            {
                var replyFromSagaMessage = new ReplyFromSagaMessage();
                Bus.Reply(replyFromSagaMessage);
                var replyToOriginatorFromSagaMessage = new ReplyToOriginatorFromSagaMessage();
                ReplyToOriginator(replyToOriginatorFromSagaMessage);
                RequestTimeout(TimeSpan.FromMilliseconds(1), new TimeoutFromSaga());
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

            public void Timeout(TimeoutFromSaga state)
            {
            }
        }

        class Mutator :
            IMutateIncomingTransportMessages
        {
            public void MutateIncoming(TransportMessage transportMessage)
            {
                var headers = transportMessage.Headers;
                if (transportMessage.IsMessageOfTye<SendFromSagaMessage>())
                {
                    var headerText = HeaderWriter.ToFriendlyString<HeaderWriterSaga>(headers);
                    SnippetLogger.Write(
                        text: headerText,
                        suffix: "Sending");
                    CountdownEvent.Signal();
                    return;
                }
                if (transportMessage.IsMessageOfTye<ReplyFromSagaMessage>())
                {
                    var headerText = HeaderWriter.ToFriendlyString<HeaderWriterSaga>(headers);
                    SnippetLogger.Write(
                        text: headerText,
                        suffix: "Replying");
                    CountdownEvent.Signal();
                    return;
                }
                if (transportMessage.IsMessageOfTye<ReplyToOriginatorFromSagaMessage>())
                {
                    var headerText = HeaderWriter.ToFriendlyString<HeaderWriterSaga>(headers);
                    SnippetLogger.Write(
                        text: headerText,
                        suffix: "ReplyingToOriginator");
                    CountdownEvent.Signal();
                    return;
                }

                if (transportMessage.IsMessageOfTye<TimeoutFromSaga>())
                {
                    var headerText = HeaderWriter.ToFriendlyString<HeaderWriterSaga>(headers);
                    SnippetLogger.Write(
                        text: headerText,
                        suffix: "Timeout");
                    CountdownEvent.Signal();
                }
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
