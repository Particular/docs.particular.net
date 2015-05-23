using System;
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
    public static ManualResetEvent ResetEvent;
    public static UnicastBus Bus;
    string endpointName = "HeaderWriterSagaV5";

    [Test]
    public void Write()
    {
        QueueCreation.DeleteQueuesForEndpoint(endpointName);
        try
        {
            ResetEvent = new ManualResetEvent(false);
            BusConfiguration config = new BusConfiguration();
            config.EndpointName(endpointName);
            config.TypesToScan(TypeScanner.TypesFor<HeaderWriterSaga>());
            config.EnableInstallers();
            config.UsePersistence<InMemoryPersistence>();
            config.RegisterComponents(c => c.ConfigureComponent<Mutator>(DependencyLifecycle.InstancePerCall));
            using (IStartableBus startableBus = NServiceBus.Bus.Create(config))
            using (Bus = (UnicastBus) startableBus.Start())
            {
                Bus.SendLocal(new StartSaga1Message());
                ResetEvent.WaitOne();
            }
        }
        finally
        {
            QueueCreation.DeleteQueuesForEndpoint(endpointName);
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
            HeaderWriterSaga.Bus.SendLocal(new SendFromSagaMessage());
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

    class Saga2 : Saga<Saga2.SagaData>, IAmStartedByMessages<SendFromSagaMessage>
    {
        public void Handle(SendFromSagaMessage message)
        {
            HeaderWriterSaga.Bus.Reply(new ReplyFromSagaMessage());
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
    }

    class Mutator : IMutateIncomingTransportMessages
    {
        public void MutateIncoming(TransportMessage transportMessage)
        {
            if (transportMessage.IsMessageOfTye<SendFromSagaMessage>())
            {
                string headerText = HeaderWriter.ToFriendlyString<HeaderWriterSaga>(transportMessage.Headers);
                SnippetLogger.Write(text: headerText, suffix: "Sending");
                return;
            }
            if (transportMessage.IsMessageOfTye<ReplyFromSagaMessage>())
            {
                string headerText = HeaderWriter.ToFriendlyString<HeaderWriterSaga>(transportMessage.Headers);
                SnippetLogger.Write(text: headerText, suffix: "Replying");
                ResetEvent.Set();
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

}