﻿namespace Core7.Headers.Writers
{
    using System;
    using System.Threading;
    using System.Threading.Tasks;
    using Common;
    using NServiceBus;
    using NServiceBus.MessageMutator;
    using NUnit.Framework;

    [TestFixture]
    public class HeaderWriterDefer
    {
        static ManualResetEvent ManualResetEvent = new ManualResetEvent(false);
        public static bool Received;
        static string EndpointName = "HeaderWriterDeferV7";

        [Test]
        public async Task Write()
        {
            var endpointConfiguration = new EndpointConfiguration(EndpointName);
            var typesToScan = TypeScanner.NestedTypes<HeaderWriterDefer>();
            endpointConfiguration.SetTypesToScan(typesToScan);
            endpointConfiguration.SendFailedMessagesTo("error");
            endpointConfiguration.EnableInstallers();
            endpointConfiguration.UsePersistence<InMemoryPersistence>();
            endpointConfiguration.RegisterMessageMutator(new Mutator());
            var routing = endpointConfiguration.UseTransport<LearningTransport>().Routing();
            routing.RouteToEndpoint(GetType().Assembly, EndpointName);

            var endpointInstance = await Endpoint.Start(endpointConfiguration);

            var options = new SendOptions();
            options.DelayDeliveryWith(TimeSpan.FromMilliseconds(10));
            await endpointInstance.Send(new MessageToSend(), options);
            ManualResetEvent.WaitOne();
            await endpointInstance.Stop();
        }

        class MessageToSend :
            IMessage
        {
        }

        class MessageHandler :
            IHandleMessages<MessageToSend>
        {
            public Task Handle(MessageToSend message, IMessageHandlerContext context)
            {
                return Task.CompletedTask;
            }
        }

        class Mutator :
            IMutateIncomingTransportMessages
        {
            public Task MutateIncoming(MutateIncomingTransportMessageContext context)
            {
                var headerText = HeaderWriter.ToFriendlyString<HeaderWriterDefer>(context.Headers);
                SnippetLogger.Write(headerText);
                ManualResetEvent.Set();
                return Task.CompletedTask;
            }
        }
    }
}