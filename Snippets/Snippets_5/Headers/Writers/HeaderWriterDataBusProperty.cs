namespace Snippets5.Headers.Writers
{
    using System;
    using System.Collections.Generic;
    using System.Text;
    using System.Threading;
    using NServiceBus;
    using NServiceBus.MessageMutator;
    using NUnit.Framework;
    using Operations.Msmq;

    [TestFixture]
    public class HeaderWriterDataBusProperty
    {
        static ManualResetEvent ManualResetEvent = new ManualResetEvent(false);

        string endpointName = "HeaderWriterDataBusPropertyV5";

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
            config.UseDataBus<FileShareDataBus>().BasePath(@"..\..\..\storage");
            IEnumerable<Type> typesToScan = TypeScanner.NestedTypes<HeaderWriterDataBusProperty>(typeof(ConfigErrorQueue));
            config.TypesToScan(typesToScan);
            config.EnableInstallers();
            config.UsePersistence<InMemoryPersistence>();
            config.RegisterComponents(c => c.ConfigureComponent<Mutator>(DependencyLifecycle.InstancePerCall));
            using (IBus bus = Bus.Create(config).Start())
            {
                bus.SendLocal(new MessageToSend
                {
                    LargeProperty1 = new DataBusProperty<byte[]>(new byte[10]),
                    LargeProperty2 = new DataBusProperty<byte[]>(new byte[10]) 
                });
                ManualResetEvent.WaitOne();
            }
        }

        class MessageToSend : IMessage
        {
            public DataBusProperty<byte[]> LargeProperty1 { get; set; }
            public DataBusProperty<byte[]> LargeProperty2 { get; set; }
        }
    
        class MessageHandler : IHandleMessages<MessageToSend>
        {
            public void Handle(MessageToSend message)
            {
                throw new Exception("error");
            }
        }

        class Mutator : IMutateIncomingTransportMessages
        {
            public void MutateIncoming(TransportMessage transportMessage)
            {
                string headerText = HeaderWriter.ToFriendlyString<HeaderWriterSend>(transportMessage.Headers);
                SnippetLogger.Write(headerText, version: "All");
                SnippetLogger.Write(Encoding.Default.GetString(transportMessage.Body), version: "All",suffix:"Body");
                ManualResetEvent.Set();
            }
        }
    }
}