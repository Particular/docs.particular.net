using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.IO;
using System.Threading.Tasks;
using NServiceBus.Transport.SqlServerNative;

public class DelayedQueue
{
    SqlConnection sqlConnection = null;

    async Task CreateQueue()
    {
        #region CreateDelayedQueue

        var queueManager = new DelayedQueueManager("endpointTable.Delayed", sqlConnection);
        await queueManager.Create().ConfigureAwait(false);

        #endregion
    }

    async Task DeleteQueue()
    {
        #region DeleteDelayedQueue

        var queueManager = new DelayedQueueManager("endpointTable.Delayed", sqlConnection);
        await queueManager.Drop()
            .ConfigureAwait(false);

        #endregion
    }

    async Task Send()
    {
        string headers = null;
        byte[] body = null;

        #region SendDelayed

        var queueManager = new DelayedQueueManager("endpointTable.Delayed", sqlConnection);
        var message = new OutgoingDelayedMessage(
            due: DateTime.UtcNow.AddDays(1),
            headers: headers,
            bodyBytes: body);
        await queueManager.Send(message)
            .ConfigureAwait(false);

        #endregion
    }

    async Task SendBatch()
    {
        string headers1 = null;
        byte[] body1 = null;
        string headers2 = null;
        byte[] body2 = null;

        #region SendDelayedBatch

        var queueManager = new DelayedQueueManager("endpointTable.Delayed", sqlConnection);
        var messages = new List<OutgoingDelayedMessage>
        {
            new OutgoingDelayedMessage(
                due: DateTime.UtcNow.AddDays(1),
                headers: headers1,
                bodyBytes: body1),
            new OutgoingDelayedMessage(
                due: DateTime.UtcNow.AddDays(1),
                headers: headers2,
                bodyBytes: body2),
        };
        await queueManager.Send(messages)
            .ConfigureAwait(false);

        #endregion
    }

    async Task Read()
    {
        #region ReadDelayed

        var queueManager = new DelayedQueueManager("endpointTable", sqlConnection);
        var message = await queueManager.Read(rowVersion: 10)
            .ConfigureAwait(false);

        Console.WriteLine(message.Headers);
        using (var reader = new StreamReader(message.Body))
        {
            var bodyText = await reader.ReadToEndAsync()
                .ConfigureAwait(false);
            Console.WriteLine(bodyText);
        }

        #endregion
    }

    async Task ReadBatch()
    {
        #region ReadDelayedBatch

        var queueManager = new DelayedQueueManager("endpointTable", sqlConnection);
        var result = await queueManager.Read(
                size: 5,
                startRowVersion: 10,
                action: async message =>
                {
                    Console.WriteLine(message.Headers);
                    using (var reader = new StreamReader(message.Body))
                    {
                        var bodyText = await reader.ReadToEndAsync()
                            .ConfigureAwait(false);
                        Console.WriteLine(bodyText);
                    }
                })
            .ConfigureAwait(false);

        Console.WriteLine(result.Count);
        Console.WriteLine(result.LastRowVersion);

        #endregion
    }

    async Task Consume()
    {
        #region ConsumeDelayed

        var queueManager = new DelayedQueueManager("endpointTable", sqlConnection);
        var message = await queueManager.Consume()
            .ConfigureAwait(false);

        Console.WriteLine(message.Headers);
        using (var reader = new StreamReader(message.Body))
        {
            var bodyText = await reader.ReadToEndAsync()
                .ConfigureAwait(false);
            Console.WriteLine(bodyText);
        }

        #endregion
    }

    async Task ConsumeBatch()
    {
        #region ConsumeDelayedBatch

        var queueManager = new DelayedQueueManager("endpointTable", sqlConnection);
        var result = await queueManager.Consume(
                size: 5,
                action: async message =>
                {
                    Console.WriteLine(message.Headers);
                    using (var reader = new StreamReader(message.Body))
                    {
                        var bodyText = await reader.ReadToEndAsync()
                            .ConfigureAwait(false);
                        Console.WriteLine(bodyText);
                    }
                })
            .ConfigureAwait(false);

        Console.WriteLine(result.Count);
        Console.WriteLine(result.LastRowVersion);

        #endregion
    }
}