namespace EventStoreTransport_1
{
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using NServiceBus;
    using NServiceBus.Extensibility;
    using NServiceBus.Raw;
    using NServiceBus.Routing;
    using NServiceBus.Transport;

    public class API
    {
        static async Task Start()
        {
            #region Configuration

            var senderConfig = RawEndpointConfiguration.Create("EndpointName", OnMessage);
            senderConfig.UseTransport<MsmqTransport>();
            senderConfig.SendFailedMessagesTo("error");

            var sender = await RawEndpoint.Start(senderConfig)
                .ConfigureAwait(false);

            #endregion

            #region Sending

            var body = Serialize();
            var headers = new Dictionary<string, string>
            {
                ["SomeHeader"] = "SomeValue"
            };
            var request = new OutgoingMessage(
                messageId: Guid.NewGuid().ToString(),
                headers: headers, 
                body: body);

            var operation = new TransportOperation(
                request,
                new UnicastAddressTag("Receiver"));

            await sender.SendRaw(
                    new TransportOperations(operation),
                    new TransportTransaction(),
                    new ContextBag())
                .ConfigureAwait(false);

            #endregion
        }

        #region Receiving

        static Task OnMessage(MessageContext context, IDispatchMessages dispatcher)
        {
            var message = Deserialize(context.Body);

            Console.WriteLine(message);
            // Can use dispatcher to send messages from here

            return Task.CompletedTask;
        }

        #endregion

        static byte[] Serialize()
        {
            throw new NotImplementedException();
        }

        static string Deserialize(byte[] body)
        {
            throw new NotImplementedException();
        }
    }
}