using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.WindowsAzure.Storage.Table;
using NServiceBus;
using NServiceBus.Logging;
using Shared;

public class Processor
{
    static ILog log = LogManager.GetLogger<Processor>();
    IEndpointInstance endpoint;
    CloudTable table;

    static readonly ConcurrentQueue<RequestRecord> toProcess = new ConcurrentQueue<RequestRecord>();

    public void Start(IEndpointInstance endpointInstance, CancellationToken token)
    {
        endpoint = endpointInstance;
        var cloudTableClient = StorageHelper.GetTableClient();
        table = cloudTableClient.GetTableReference(Constants.TableName);
        table.CreateIfNotExists();

        var task = new Task(() =>
        {
            #region tasks

            StartPolling(token);
            StartProcessing(token);

            #endregion
        }, token);
        task.Start();
    }

    async Task StartPolling(CancellationToken token)
    {
        // should do CancellationToken and handle TaskCanceledException
        while (!token.IsCancellationRequested)
        {
            await LoadRequests()
                .ConfigureAwait(false);
            await Task.Delay(TimeSpan.FromSeconds(Constants.PollingFrequencyInSeconds), token)
                .ConfigureAwait(false);
        }
    }

    async Task StartProcessing(CancellationToken token)
    {
        while (!token.IsCancellationRequested)
        {
            RequestRecord request;
            while (toProcess.TryPeek(out request))
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

    async Task LoadRequests()
    {
        var partitionKeyFilter = TableQuery.GenerateFilterCondition(nameof(RequestRecord.PartitionKey), QueryComparisons.Equal, Constants.PartitionKey);
        var statusFilter = TableQuery.GenerateFilterCondition(nameof(RequestRecord.Status), QueryComparisons.Equal, Status.Pending.ToString());
        var filter = TableQuery.CombineFilters(partitionKeyFilter, TableOperators.And, statusFilter);
        var query = new TableQuery<RequestRecord>().Where(filter);

        var records = new List<RequestRecord>();
        TableContinuationToken continuationToken = null;
        do
        {
            var tableQuerySegment = await table.ExecuteQuerySegmentedAsync(query, continuationToken)
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