using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Azure.Cosmos.Table;
using NServiceBus;
using NServiceBus.Logging;

public class Processor
{
    static ILog log = LogManager.GetLogger<Processor>();
    IEndpointInstance endpoint;
    CloudTable table;
    CancellationTokenSource cancellationTokenSource;

    ConcurrentQueue<RequestRecord> toProcess = new ConcurrentQueue<RequestRecord>();
    CancellationToken cancellationToken;
    Task pollingTask;
    Task processingTask;

    public async Task Start(IEndpointInstance endpointInstance)
    {
        cancellationTokenSource = new CancellationTokenSource();
        cancellationToken = cancellationTokenSource.Token;
        endpoint = endpointInstance;

        var cloudTableClient = StorageHelper.GetTableClient();
        table = cloudTableClient.GetTableReference(Constants.TableName);
        await table.CreateIfNotExistsAsync(cancellationToken)
            .ConfigureAwait(false);

        #region tasks
        pollingTask = Task.Run(() => StartPolling(cancellationToken));
        processingTask = Task.Run(() => StartProcessing(cancellationToken));
        #endregion
    }

    public async Task Stop()
    {
        cancellationTokenSource.Cancel();
        await Task.WhenAll(pollingTask, processingTask)
            .ConfigureAwait(false);
        cancellationTokenSource.Dispose();
    }

    async Task StartPolling(CancellationToken token)
    {
        // should handle TaskCanceledException
        while (!token.IsCancellationRequested)
        {
            await LoadRequests(token)
                .ConfigureAwait(false);
            await Task.Delay(Constants.PollingFrequency, token)
                .ConfigureAwait(false);
        }
    }

    async Task StartProcessing(CancellationToken token)
    {
        while (!token.IsCancellationRequested)
        {
            while (toProcess.TryPeek(out var request))
            {
                try
                {
                    log.Info($"Processing request with ID {request.RequestId} and estimated processing time {request.EstimatedProcessingTime}.");
                    request.StartedAt = DateTime.UtcNow;

                    #region failed-scenario

                    // emulate failure
                    if (DateTime.UtcNow.Ticks % 2 == 0)
                    {
                        throw new Exception("Some exception during processing.");
                    }

                    #endregion

                    // process
                    var estimatedProcessingTime = TimeSpan.Parse(request.EstimatedProcessingTime);
                    await Task.Delay(estimatedProcessingTime, token)
                        .ConfigureAwait(false);
                    log.Info($"Request with ID {request.RequestId} processed.");

                    request.Status = Status.Finished.ToString();
                    request.FinishedAt = DateTime.UtcNow;
                    await table.ExecuteAsync(TableOperation.Merge(request), token)
                        .ConfigureAwait(false);
                    toProcess.TryDequeue(out request);
                    var processingFinished = new LongProcessingFinished
                    {
                        Id = request.RequestId
                    };
                    await endpoint.Publish(processingFinished)
                        .ConfigureAwait(false);
                }
                catch (Exception ex)
                {
                    log.Info($"Request with ID {request.RequestId} threw an exception.");
                    request.Status = Status.Failed.ToString();
                    await table.ExecuteAsync(TableOperation.Merge(request), token)
                        .ConfigureAwait(false);
                    toProcess.TryDequeue(out request);
                    var processingFailed = new LongProcessingFailed
                    {
                        Id = request.RequestId,
                        Reason = ex.Message
                    };
                    await endpoint.Publish(processingFailed)
                        .ConfigureAwait(false);
                }
            }
        }
    }

    async Task LoadRequests(CancellationToken token)
    {
        var partitionKeyFilter = TableQuery.GenerateFilterCondition(nameof(RequestRecord.PartitionKey), QueryComparisons.Equal, Constants.PartitionKey);
        var statusFilter = TableQuery.GenerateFilterCondition(nameof(RequestRecord.Status), QueryComparisons.Equal, Status.Pending.ToString());
        var filter = TableQuery.CombineFilters(partitionKeyFilter, TableOperators.And, statusFilter);
        var query = new TableQuery<RequestRecord>()
            .Where(filter);

        var records = new List<RequestRecord>();
        TableContinuationToken continuationToken = null;
        do
        {
            var tableQuerySegment = await table.ExecuteQuerySegmentedAsync(query, continuationToken, token)
                .ConfigureAwait(false);
            continuationToken = tableQuerySegment.ContinuationToken;
            records.AddRange(tableQuerySegment);
        } while (continuationToken != null);

        foreach (var record in records)
        {
            if (toProcess.All(r => r.RequestId != record.RequestId))
            {
                toProcess.Enqueue(record);
            }
        }
    }
}
