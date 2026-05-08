namespace Core9;

using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using NServiceBus;
using NServiceBus.Routing;
using NServiceBus.Transport;

public class RawUsage
{
    static async Task Start()
    {
        #region RawConfiguration

        var transport = new LearningTransport();

        var hostSettings = new HostSettings(
            name: "MyRawEndpoint",
            hostDisplayName: "My Raw Endpoint",
            startupDiagnostic: new StartupDiagnosticEntries(),
            criticalErrorAction: (message, exception, token) =>
            {
                Console.WriteLine("Critical error: " + exception);
            },
            setupInfrastructure: true);

        var infrastructure = await transport.Initialize(hostSettings, new[]
            {
                new ReceiveSettings(
                    id: "Primary",
                    receiveAddress: new QueueAddress("MyQueue"),
                    usePublishSubscribe: false,
                    purgeOnStartup: false,
                    errorQueue: "error")
            }, new string[0]);

        var sender = infrastructure.Dispatcher;

        #endregion

        #region RawSending

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
                transaction: new TransportTransaction());

        #endregion

        #region RawReceiving

        var receiver = infrastructure.Receivers["Primary"];
        await receiver.Initialize(new PushRuntimeSettings(8),
            onMessage: (context, token) =>
            {
                var message = Deserialize(context.Body);
                return Console.Out.WriteLineAsync(message);
            },
            onError: (context, token) => Task.FromResult(ErrorHandleResult.RetryRequired));

        await receiver.StartReceive();

        #endregion

        #region RawShutdown

        await receiver.StopReceive();
        await infrastructure.Shutdown();

        #endregion
    }
    static byte[] Serialize()
    {
        throw new NotImplementedException();
    }

    static string Deserialize(ReadOnlyMemory<byte> body)
    {
        throw new NotImplementedException();
    }
}
