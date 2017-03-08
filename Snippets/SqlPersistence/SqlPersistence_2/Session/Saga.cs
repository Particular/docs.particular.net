using System.Threading.Tasks;
using NServiceBus;
using NServiceBus.Logging;
using NServiceBus.Persistence.Sql;

#region saga-sqlPersistenceSession

[SqlSaga(
    correlationProperty: nameof(SagaData.CorrelationProperty)
)]
public class SagaThatUsesSession :
    Saga<SagaThatUsesSession.SagaData>,
    IHandleMessages<MyMessage>
{
    static ILog log = LogManager.GetLogger<HandlerThatUsesSession>();

    public Task Handle(MyMessage message, IMessageHandlerContext context)
    {
        var sqlPersistenceSession = context.SynchronizedStorageSession.SqlPersistenceSession();
        log.Info(sqlPersistenceSession.Connection.ConnectionString);
        // use Connection and/or Transaction of ISqlStorageSession to persist or query the database
        return Task.CompletedTask;
    }

    #endregion

    protected override void ConfigureHowToFindSaga(SagaPropertyMapper<SagaData> mapper)
    {
    }

    public class SagaData :
        ContainSagaData
    {
        public object CorrelationProperty { get; }
    }
}