using System.Threading;
using NServiceBus;
using NServiceBus.MessageMutator;
using NUnit.Framework;
using Operations.Msmq;

[TestFixture]
public class HeaderWriterReply
{
    public static ManualResetEvent ManualResetEvent;
    static IBus Bus;
    string endpointName = "HeaderWriterReplyV5";

    [SetUp]
    [TearDown]
    public void Setup()
    {
        QueueCreation.DeleteQueuesForEndpoint(endpointName);
    }

    [Test]
    public void Write()
    {
        ManualResetEvent = new ManualResetEvent(false);
        BusConfiguration busConfiguration = new BusConfiguration();
        busConfiguration.EndpointName(endpointName);
        busConfiguration.TypesToScan(TypeScanner.TypesFor<HeaderWriterReply>());
        busConfiguration.EnableInstallers();
        busConfiguration.UsePersistence<InMemoryPersistence>();
        busConfiguration.RegisterComponents(c => c.ConfigureComponent<Mutator>(DependencyLifecycle.InstancePerCall));
        using (IStartableBus startableBus = NServiceBus.Bus.Create(busConfiguration))
        using (Bus = startableBus.Start())
        {
            Bus.SendLocal(new MessageToSend());
            ManualResetEvent.WaitOne();
        }
    }

    class MessageToSend : IMessage
    {
    }

    class MessageHandler : IHandleMessages<MessageToSend>
    {
        public void Handle(MessageToSend message)
        {
            Bus.Reply(new MessageToReply());
            Bus.Return(100);
        }
    }

    class MessageToReply : IMessage
    {
    }

    class Mutator : IMutateIncomingTransportMessages
    {
        public void MutateIncoming(TransportMessage transportMessage)
        {
            if (transportMessage.IsMessageOfTye<MessageToReply>())
            {
                string headerText = HeaderWriter.ToFriendlyString<HeaderWriterReply>(transportMessage.Headers);
                SnippetLogger.Write(text: headerText, suffix: "Replying");
                ManualResetEvent.Set();
            }
            if (transportMessage.IsMessageOfTye<MessageToSend>())
            {
                string headerText = HeaderWriter.ToFriendlyString<HeaderWriterReply>(transportMessage.Headers);
                SnippetLogger.Write(text: headerText, suffix: "Sending");
            }
        }
    }
}