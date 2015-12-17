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
    public class HeaderWriterDataBusConvention
    {
        static ManualResetEvent ManualResetEvent = new ManualResetEvent(false);

        string endpointName = "HeaderWriterDataBusConventionV5";

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
            IEnumerable<Type> typesToScan = TypeScanner.NestedTypes<HeaderWriterDataBusConvention>(typeof(ConfigErrorQueue));
            config.TypesToScan(typesToScan);
            config.EnableInstallers();
            config.Conventions().DefiningDataBusPropertiesAs(x => x.Name.StartsWith("LargeProperty"));
            config.UsePersistence<InMemoryPersistence>();
            config.RegisterComponents(c => c.ConfigureComponent<Mutator>(DependencyLifecycle.InstancePerCall));
            using (IBus bus = Bus.Create(config).Start())
            {
                bus.SendLocal(new MessageToSend
                {
                    LargeProperty1 = new byte[10],
                    LargeProperty2 = new byte[10] 
                });
                ManualResetEvent.WaitOne();
            }
        }

        class MessageToSend : IMessage
        {
            public byte[] LargeProperty1 { get; set; }
            public byte[] LargeProperty2 { get; set; }
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
                string headerText = HeaderWriter.ToFriendlyString<HeaderWriterSend>(transportMessage.Headers)
                    .Replace(typeof(MessageToSend).FullName, "MessageToSend");
                SnippetLogger.Write(headerText, version: "All");
                SnippetLogger.Write(Encoding.Default.GetString(transportMessage.Body), version: "All", suffix: "Body");
                ManualResetEvent.Set();
            }
        }
    }
}