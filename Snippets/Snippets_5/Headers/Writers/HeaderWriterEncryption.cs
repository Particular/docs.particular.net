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
            BusConfiguration config = new BusConfiguration();
            config.EndpointName(endpointName);
#pragma warning disable 618
            config.RijndaelEncryptionService("gdDbqRpqdRbTs3mhdZh9qCaDaxJXl+e6", Encoding.ASCII.GetBytes("aaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaa"));
            config.Conventions().DefiningEncryptedPropertiesAs(info => info.Name.StartsWith("EncryptedProperty"));
#pragma warning restore 618
            IEnumerable<Type> typesToScan = TypeScanner.NestedTypes<HeaderWriterEncryption>(typeof(ConfigErrorQueue));
            config.TypesToScan(typesToScan);
            config.EnableInstallers();
            config.UsePersistence<InMemoryPersistence>();
            config.RegisterComponents(c => c.ConfigureComponent<Mutator>(DependencyLifecycle.InstancePerCall));
            using (IBus bus = Bus.Create(config).Start())
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
                throw new Exception("error");
            }
        }

        class Mutator : IMutateIncomingTransportMessages
        {
            public void MutateIncoming(TransportMessage transportMessage)
            {
                string headerText = HeaderWriter.ToFriendlyString<HeaderWriterSend>(transportMessage.Headers);
                SnippetLogger.Write(headerText, version: "All");
                //SnippetLogger.Write(Encoding.Default.GetString(transportMessage.Body), version: "All",suffix:"Body");
                ManualResetEvent.Set();
            }
        }
    }
}