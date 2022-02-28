﻿using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using NServiceBus;
using NServiceBus.Routing;
using NServiceBus.Transport;

public class Usage
{
    static async Task Start()
    {
        #region Configuration

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
            }, new string[0])
            .ConfigureAwait(false);

        var sender = infrastructure.Dispatcher;

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
                transaction: new TransportTransaction())
            .ConfigureAwait(false);

        #endregion

        #region Receiving

        var receiver = infrastructure.Receivers["Primary"];
        await receiver.Initialize(new PushRuntimeSettings(8),
            onMessage: (context, token) =>
            {
                var message = Deserialize(context.Body);
                return Console.Out.WriteLineAsync(message);
            },
            onError: (context, token) => Task.FromResult(ErrorHandleResult.RetryRequired));

        await receiver.StartReceive().ConfigureAwait(false);

        #endregion

        #region Shutdown

        await receiver.StopReceive().ConfigureAwait(false);
        await infrastructure.Shutdown().ConfigureAwait(false);

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
