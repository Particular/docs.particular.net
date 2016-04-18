namespace Core5.Headers.Writers
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
            BusConfiguration busConfiguration = new BusConfiguration();
            busConfiguration.EndpointName(endpointName);
            busConfiguration.UseDataBus<FileShareDataBus>().BasePath(@"..\..\..\storage");
            IEnumerable<Type> typesToScan = TypeScanner.NestedTypes<HeaderWriterDataBusConvention>(typeof(ConfigErrorQueue));
            busConfiguration.TypesToScan(typesToScan);
            busConfiguration.EnableInstallers();
            busConfiguration.Conventions().DefiningDataBusPropertiesAs(x => x.Name.StartsWith("LargeProperty"));
            busConfiguration.UsePersistence<InMemoryPersistence>();
            busConfiguration.RegisterComponents(c => c.ConfigureComponent<Mutator>(DependencyLifecycle.InstancePerCall));
            using (IBus bus = Bus.Create(busConfiguration).Start())
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
                string headerText = HeaderWriter.ToFriendlyString<HeaderWriterDataBusConvention>(transportMessage.Headers)
                    .Replace(typeof(MessageToSend).FullName, "MessageToSend");
                SnippetLogger.Write(headerText, version: "5");
                SnippetLogger.Write(Encoding.Default.GetString(transportMessage.Body), version: "5", suffix: "Body");
                ManualResetEvent.Set();
            }
        }
    }
}