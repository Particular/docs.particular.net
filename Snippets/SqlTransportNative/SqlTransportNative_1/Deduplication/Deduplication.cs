using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Threading.Tasks;
using NServiceBus.Transport.SqlServerNative;

public class Deduplication
{
    SqlConnection sqlConnection = null;
    string connectionString = null;

    async Task CreateTable()
    {
        #region CreateDeduplicationTable

        var queueManager = new DeduplicationManager(sqlConnection, "DeduplicationTable");
        await queueManager.Create().ConfigureAwait(false);

        #endregion
    }

    async Task DeleteTable()
    {
        #region DeleteDeduplicationTable

        var queueManager = new DeduplicationManager(sqlConnection, "DeduplicationTable");
        await queueManager.Drop()
            .ConfigureAwait(false);

        #endregion
    }

    async Task Send()
    {
        string headers = null;
        byte[] body = null;

        #region SendWithDeduplication

        var queueManager = new QueueManager("endpointTable", sqlConnection, "DeduplicationTable");
        var message = new OutgoingMessage(
            id: Guid.NewGuid(),
            headers: headers,
            bodyBytes: body);
        await queueManager.Send(message)
            .ConfigureAwait(false);

        #endregion

    }

    async Task DeduplicationCleanerJob()
    {
        #region DeduplicationCleanerJobStart

        var cleaner = new DeduplicationCleanerJob(
            table: "Deduplication",
            connectionBuilder: cancellation =>
            {
                return ConnectionHelpers.OpenConnection(connectionString, cancellation);
            },
            criticalError: (message, exception) => { },
            expireWindow: TimeSpan.FromHours(1),
            frequencyToRunCleanup: TimeSpan.FromMinutes(10));
        cleaner.Start();
        #endregion

        #region DeduplicationCleanerJobStop

        await cleaner.Stop().ConfigureAwait(false);
        #endregion
    }

    async Task SendBatch()
    {
        string headers1 = null;
        byte[] body1 = null;
        string headers2 = null;
        byte[] body2 = null;

        #region SendBatchWithDeduplication

        var queueManager = new QueueManager("endpointTable", sqlConnection, "DeduplicationTable");
        var messages = new List<OutgoingMessage>
        {
            new OutgoingMessage(
                id: Guid.NewGuid(),
                headers: headers1,
                bodyBytes: body1),
            new OutgoingMessage(
                id: Guid.NewGuid(),
                headers: headers2,
                bodyBytes: body2),
        };
        await queueManager.Send(messages)
            .ConfigureAwait(false);

        #endregion
    }
}