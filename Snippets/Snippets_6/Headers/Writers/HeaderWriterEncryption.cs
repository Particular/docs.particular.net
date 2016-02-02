﻿namespace Snippets6.Headers.Writers
{
    using System;
    using System.Collections.Generic;
    using System.Text;
    using System.Threading;
    using System.Threading.Tasks;
    using NServiceBus;
    using NServiceBus.MessageMutator;
    using NUnit.Framework;
    using Operations.Msmq;

    [TestFixture]
    public class HeaderWriterEncryption
    {
        static ManualResetEvent ManualResetEvent = new ManualResetEvent(false);

        string endpointName = "HeaderWriterEncryptionV6";

        [SetUp]
        [TearDown]
        public void Setup()
        {
            QueueDeletion.DeleteQueuesForEndpoint(endpointName);
        }

        [Test]
        public async Task Write()
        {
            BusConfiguration config = new BusConfiguration();
            config.EndpointName(endpointName);
            config.RijndaelEncryptionService("gdDbqRpqdRbTs3mhdZh9qCaDaxJXl+e6");
            config.Conventions().DefiningEncryptedPropertiesAs(info => info.Name.StartsWith("EncryptedProperty"));
            IEnumerable<Type> typesToScan = TypeScanner.NestedTypes<HeaderWriterEncryption>();
            config.SetTypesToScan(typesToScan);
            config.SendFailedMessagesTo("error");
            config.EnableInstallers();
            config.UsePersistence<InMemoryPersistence>();
            config.RegisterComponents(c => c.ConfigureComponent<Mutator>(DependencyLifecycle.InstancePerCall));
            IEndpointInstance endpoint = await Endpoint.Start(config);
            await endpoint.SendLocal(new MessageToSend
            {
                EncryptedProperty1 = "String 1",
                EncryptedProperty2 = "String 2"
            });
            ManualResetEvent.WaitOne();
        }

        class MessageToSend : IMessage
        {
            public string EncryptedProperty1 { get; set; }
            public string EncryptedProperty2 { get; set; }
        }

        class MessageHandler : IHandleMessages<MessageToSend>
        {
            public Task Handle(MessageToSend message, IMessageHandlerContext context)
            {
                return Task.FromResult(0);
            }
        }

        class Mutator : IMutateIncomingTransportMessages
        {
            public Task MutateIncoming(MutateIncomingTransportMessageContext context)
            {
                string headerText = HeaderWriter.ToFriendlyString<HeaderWriterEncryption>(context.Headers);
                SnippetLogger.Write(headerText, version: "6");
                SnippetLogger.Write(Encoding.Default.GetString(context.Body), version: "6", suffix: "Body");
                ManualResetEvent.Set();
                return Task.FromResult(0);
            }
        }
    }
}