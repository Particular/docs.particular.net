using System;
using System.Threading.Tasks;
using Microsoft.Azure.Cosmos;
using NServiceBus;

public class ProcessOrderHandler : IHandleMessages<ProcessOrder>
{
    #region Handler
    public Task Handle(ProcessOrder message, IMessageHandlerContext context)
    {
        Console.WriteLine($"Processing MessageId {context.MessageId} for OrderId {message.OrderId}");

        var processedOrder = new ProcessedOrder
        {
            Id = Guid.NewGuid(),
            OrderId = message.OrderId,
            MessageId = context.MessageId,
            ProcessedAt = DateTimeOffset.UtcNow
        };

        // Added to the same transactional batch as the Outbox record
        var session = context.SynchronizedStorageSession.CosmosPersistenceSession();
        session.Batch.CreateItem(processedOrder, new TransactionalBatchItemRequestOptions
        {
            EnableContentResponseOnWrite = false
        });

        return Task.CompletedTask;
    }
    #endregion
}
