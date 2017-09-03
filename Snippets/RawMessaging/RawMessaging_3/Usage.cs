using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using NServiceBus.Extensibility;
using NServiceBus.Raw;
using NServiceBus.Routing;
using NServiceBus.Settings;
using NServiceBus.Transport;

public class Usage
{
    static async Task Start()
    {
        #region Configuration

        var senderConfig = RawEndpointConfiguration.Create(
            endpointName: "EndpointName",
            onMessage: OnMessage,
            poisonMessageQueue: "error");
        senderConfig.UseTransport<MsmqTransport>();

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

        await sender.Dispatch(
                outgoingMessages: new TransportOperations(operation),
                transaction: new TransportTransaction(),
                context: new ContextBag())
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

class MsmqTransport: TransportDefinition
{
    public override TransportInfrastructure Initialize(SettingsHolder settings, string connectionString)
    {
        throw new NotImplementedException();
    }

    public override string ExampleConnectionStringForErrorMessage { get; }
}
