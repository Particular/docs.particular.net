using System.Threading.Tasks;
using NServiceBus;
using NServiceBus.Logging;
using NServiceBus.Persistence.Sql;

#region saga-sqlPersistenceSession

public class SagaThatUsesSession :
    SqlSaga<SagaThatUsesSession.SagaData>,
    IHandleMessages<MyMessage>
{
    public Task Handle(MyMessage message, IMessageHandlerContext context)
    {
        var sqlPersistenceSession = context.SynchronizedStorageSession.SqlPersistenceSession();
        log.Info(sqlPersistenceSession.Connection.ConnectionString);
        // use Connection and/or Transaction of ISqlStorageSession to persist or query the database
        return Task.CompletedTask;
    }

    #endregion

    protected override string CorrelationPropertyName => nameof(SagaData.CorrelationProperty);

    static ILog log = LogManager.GetLogger<HandlerThatUsesSession>();

    protected override void ConfigureMapping(IMessagePropertyMapper mapper)
    {
    }


    public class SagaData :
        ContainSagaData
    {
        public object CorrelationProperty { get; }
    }
}