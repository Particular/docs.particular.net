using System.Threading.Tasks;
using NServiceBus;
using NServiceBus.Logging;

#region saga-sqlPersistenceSession

public class SagaThatUsesSession :
    Saga<SagaThatUsesSession.SagaData>,
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

    static ILog log = LogManager.GetLogger<HandlerThatUsesSession>();

    protected override void ConfigureHowToFindSaga(SagaPropertyMapper<SagaData> mapper)
    {
    }


    public class SagaData :
        ContainSagaData
    {
        public object CorrelationProperty { get; }
    }
}