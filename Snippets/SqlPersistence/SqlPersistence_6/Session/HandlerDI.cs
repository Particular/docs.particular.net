using System.Data.Common;
using System.Threading.Tasks;
using NServiceBus;
using NServiceBus.Logging;
using NServiceBus.Persistence.Sql;

#region handler-sqlPersistenceSession-DI
public class HandlerThatUsesSessionViaDI :
    IHandleMessages<MyMessage>
{
    static ILog log = LogManager.GetLogger<HandlerThatUsesSessionViaDI>();
    ISqlStorageSession sqlPersistenceSession;

    public HandlerThatUsesSessionViaDI(ISqlStorageSession sqlPersistenceSession)
    {
        this.sqlPersistenceSession = sqlPersistenceSession;
    }

    public Task Handle(MyMessage message, IMessageHandlerContext context)
    {
        log.Info(sqlPersistenceSession.Connection.ConnectionString);
        // use Connection and/or Transaction of ISqlStorageSession to persist or query the database
        return Task.CompletedTask;
    }
}
#endregion

public class EndpointWithConnectionInjected
{
    public void Configure(EndpointConfiguration config)
    {
        #region sqlPersistenceSession-DI-register

        config.RegisterComponents(c =>
        {
            c.ConfigureComponent(b => b.Build<ISqlStorageSession>().Connection, 
                DependencyLifecycle.InstancePerUnitOfWork);
            c.ConfigureComponent(b => b.Build<ISqlStorageSession>().Transaction,
                DependencyLifecycle.InstancePerUnitOfWork);
        });

        #endregion
    }
}