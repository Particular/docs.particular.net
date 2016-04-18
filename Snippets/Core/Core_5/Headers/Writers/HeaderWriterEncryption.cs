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
    public class HeaderWriterEncryption
    {
        static ManualResetEvent ManualResetEvent = new ManualResetEvent(false);

        string endpointName = "HeaderWriterEncryptionV5";

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
            busConfiguration.RijndaelEncryptionService("key1", Encoding.ASCII.GetBytes("gdDbqRpqdRbTs3mhdZh9qCaDaxJXl+e6"));
            busConfiguration.Conventions().DefiningEncryptedPropertiesAs(info => info.Name.StartsWith("EncryptedProperty"));
            IEnumerable<Type> typesToScan = TypeScanner.NestedTypes<HeaderWriterEncryption>(typeof(ConfigErrorQueue));
            busConfiguration.TypesToScan(typesToScan);
            busConfiguration.EnableInstallers();
            busConfiguration.UsePersistence<InMemoryPersistence>();
            busConfiguration.RegisterComponents(c => c.ConfigureComponent<Mutator>(DependencyLifecycle.InstancePerCall));
            using (IBus bus = Bus.Create(busConfiguration).Start())
            {
                bus.SendLocal(new MessageToSend
                {
                    EncryptedProperty1 = "String 1",
                    EncryptedProperty2 = "String 2"
                });
                ManualResetEvent.WaitOne();
            }
        }

        class MessageToSend : IMessage
        {
            public string EncryptedProperty1 { get; set; }
            public string EncryptedProperty2 { get; set; }
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
                string headerText = HeaderWriter.ToFriendlyString<HeaderWriterEncryption>(transportMessage.Headers);
                SnippetLogger.Write(headerText, version: "5");
                SnippetLogger.Write(Encoding.Default.GetString(transportMessage.Body), version: "5", suffix: "Body");
                ManualResetEvent.Set();
            }
        }
    }
}