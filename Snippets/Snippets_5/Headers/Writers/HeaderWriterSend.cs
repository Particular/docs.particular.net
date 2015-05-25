using System;
using System.Collections.Generic;
using System.Threading;
using NServiceBus;
using NServiceBus.MessageMutator;
using NUnit.Framework;
using Operations.Msmq;

[TestFixture]
public class HeaderWriterSend
{
    public static ManualResetEvent ManualResetEvent;

    string endpointName = "HeaderWriterSendV5";

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

        BusConfiguration config = new BusConfiguration();
        config.EndpointName(endpointName);
        IEnumerable<Type> typesToScan = TypeScanner.NestedTypes<HeaderWriterSend>(typeof(ConfigErrorQueue));
        config.TypesToScan(typesToScan);
        config.EnableInstallers();
        config.UsePersistence<InMemoryPersistence>();
        config.RegisterComponents(c => c.ConfigureComponent<Mutator>(DependencyLifecycle.InstancePerCall));
        using (IStartableBus startableBus = Bus.Create(config))
        using (IBus bus = startableBus.Start())
        {
            bus.SendLocal(new MessageToSend());
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
        }
    }

    class Mutator : IMutateIncomingTransportMessages
    {
        public void MutateIncoming(TransportMessage transportMessage)
        {
            string headerText = HeaderWriter.ToFriendlyString<HeaderWriterSend>(transportMessage.Headers);
            SnippetLogger.Write(headerText);
            ManualResetEvent.Set();
        }
    }
}