namespace NonDurablePersistence_3
{
    using System;
    using System.Collections.Concurrent;
    using System.Threading;
    using System.Threading.Tasks;
    using NServiceBus;
    using NServiceBus.Extensibility;
    using NServiceBus.Persistence;
    using NServiceBus.Sagas;

    #region NonDurableSagaFinderWithPersister

    class TaskIndex
    {
        public ConcurrentDictionary<Guid, Guid> ServerTaskIdToSagaId { get; } = new();
    }

    class TaskSagaFinder(TaskIndex index, ISagaPersister persister)
        : ISagaFinder<TaskSagaData, ContinueTask>
    {
        public async Task<TaskSagaData> FindBy(ContinueTask message,
            ISynchronizedStorageSession storageSession, IReadOnlyContextBag context,
            CancellationToken cancellationToken = default)
        {
            if (!index.ServerTaskIdToSagaId.TryGetValue(message.ServerTaskId, out var sagaId))
            {
                return null;
            }

            return await persister.Get<TaskSagaData>(sagaId, storageSession, (ContextBag)context, cancellationToken);
        }
    }

    class ContinueTask : IMessage
    {
        public Guid ServerTaskId { get; set; }
    }

    class TaskSagaData : ContainSagaData
    {
        public Guid ServerTaskId { get; set; }
    }

    #endregion
}
