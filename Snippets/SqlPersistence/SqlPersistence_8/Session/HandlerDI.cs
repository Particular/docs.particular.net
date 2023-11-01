using System.Data.Common;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
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
            c.AddScoped(b =>
            {
                var session = b.GetService<ISqlStorageSession>();
                var repository = new MyRepository(
                    session.Connection,
                    session.Transaction);

                //Ensure changes are saved before the transaction is committed
                session.OnSaveChanges((s, ctx) => repository.SaveChangesAsync());

                return repository;
            });
        });

        #endregion
    }

    public class MyRepository
    {
        public MyRepository(DbConnection connection, DbTransaction transaction)
        {
            throw new System.NotImplementedException();
        }

        public Task SaveChangesAsync()
        {
            throw new System.NotImplementedException();
        }
    }
}

