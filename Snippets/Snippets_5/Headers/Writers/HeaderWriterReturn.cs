using System.Threading;
using NServiceBus;
using NServiceBus.MessageMutator;
using NUnit.Framework;
using Operations.Msmq;

[TestFixture]
public class HeaderWriterReturn
{
    public static ManualResetEvent ManualResetEvent;
    static IBus Bus;
    string endpointName = "HeaderWriterReturnV5";

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
        busConfiguration.TypesToScan(TypeScanner.TypesFor<HeaderWriterReturn>());
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
            Bus.Return(100);
        }
    }

    class Mutator : IMutateIncomingTransportMessages
    {
        public void MutateIncoming(TransportMessage transportMessage)
        {
            if (transportMessage.IsMessageOfTye<MessageToSend>())
            {
                string sendingText = HeaderWriter.ToFriendlyString < HeaderWriterReturn>(transportMessage.Headers);
                SnippetLogger.Write(text: sendingText, suffix: "Sending");
            }
            else
            {
                string returnText = HeaderWriter.ToFriendlyString<HeaderWriterReturn>(transportMessage.Headers);
                SnippetLogger.Write(text: returnText, suffix: "Returning");
                ManualResetEvent.Set();
            }

        }

    }
}