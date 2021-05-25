using System;
using System.Threading.Tasks;
using Microsoft.Azure.Cosmos.Table;
using NServiceBus;
using NServiceBus.Logging;

public class LongProcessingRequestSaga :
    Saga<SagaState>,
    IAmStartedByMessages<LongProcessingRequest>,
    IHandleTimeouts<ProcessingPossiblyFailed>,
    IHandleMessages<LongProcessingFinished>,
    IHandleMessages<LongProcessingFailed>
{
    static ILog log = LogManager.GetLogger<LongProcessingRequestSaga>();
    CloudTable table;

    public LongProcessingRequestSaga()
    {
        var cloudTableClient = StorageHelper.GetTableClient();
        table = cloudTableClient.GetTableReference(Constants.TableName);
        table.CreateIfNotExists();
    }

    public async Task Handle(LongProcessingRequest message, IMessageHandlerContext context)
    {
        log.Info($"Received LongProcessingRequest with ID {message.Id}, EstimatedProcessingTime: {message.EstimatedProcessingTime}.");

        #region setting-timeout

        var timeoutToBeInvokedAt = DateTime.Now + message.EstimatedProcessingTime + TimeSpan.FromSeconds(10);
        var timeoutMessage = new ProcessingPossiblyFailed
        {
            Id = message.Id
        };
        await RequestTimeout(context, timeoutToBeInvokedAt, timeoutMessage)
            .ConfigureAwait(false);

        #endregion

        log.Info($"Timeout is set to be executed at {timeoutToBeInvokedAt}.");

        log.Info("Registering long running request with Processor.");

        #region enqueue-request-for-processor

        // Saga enqueues the request to process in a storage table. This is the
        // logical equivalent of adding a message to a queue. If there would be
        // business specific work to perform here, that work should be done by
        // sending a message to a handler instead and not handled in the saga.

        var request = new RequestRecord(message.Id, Status.Pending, message.EstimatedProcessingTime);
        await table.ExecuteAsync(TableOperation.Insert(request))
            .ConfigureAwait(false);

        var processingReply = new LongProcessingReply
        {
            Id = message.Id
        };
        await context.Reply(processingReply)
            .ConfigureAwait(false);

        #endregion

        log.Info($"Acknowledged LongProcessingRequest with ID {message.Id}.");
    }

    protected override void ConfigureHowToFindSaga(SagaPropertyMapper<SagaState> mapper)
    {
        mapper.ConfigureMapping<LongProcessingRequest>(message => message.Id)
            .ToSaga(state => state.LongProcessingId);
        mapper.ConfigureMapping<LongProcessingFinished>(message => message.Id)
            .ToSaga(state => state.LongProcessingId);
        mapper.ConfigureMapping<LongProcessingFailed>(message => message.Id)
            .ToSaga(state => state.LongProcessingId);
    }

    public Task Timeout(ProcessingPossiblyFailed timeoutMessage, IMessageHandlerContext context)
    {
        log.Info($"Processing of LongProcessingRequest with ID {timeoutMessage.Id} has not finished in the estimated time. Try again.");

        #region on-timeout

        var processingWarning = new LongProcessingWarning
        {
            Id = timeoutMessage.Id
        };
        MarkAsComplete();
        return context.Publish(processingWarning);

        #endregion
    }

    public Task Handle(LongProcessingFinished message, IMessageHandlerContext context)
    {
        MarkAsComplete();
        return Task.CompletedTask;
    }

    public Task Handle(LongProcessingFailed message, IMessageHandlerContext context)
    {
        MarkAsComplete();
        return Task.CompletedTask;
    }
}