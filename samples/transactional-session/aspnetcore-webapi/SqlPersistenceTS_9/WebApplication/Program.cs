using Microsoft.AspNetCore.Builder;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using NServiceBus;
using NServiceBus.Persistence;
using NServiceBus.Persistence.Sql;
using NServiceBus.TransactionalSession;

// for SqlExpress use Data Source=.\SqlExpress;Initial Catalog=nservicebus;Integrated Security=True;Encrypt=false
const string ConnectionString = @"Server=localhost,1433;Initial Catalog=nservicebus;User Id=SA;Password=yourStrong(!)Password;Encrypt=false";

await using (var myDataContext = new MyDataContext(new DbContextOptionsBuilder<MyDataContext>()
                 .UseSqlServer(new SqlConnection(ConnectionString))
                 .Options))
{
    myDataContext.Database.EnsureCreated();
}

var hostBuilder = WebApplication.CreateBuilder();

#region txsession-nsb-configuration
var endpointConfiguration = new EndpointConfiguration("Samples.ASPNETCore.Sender");

endpointConfiguration.UseSerialization<SystemJsonSerializer>();

endpointConfiguration.EnableInstallers();

endpointConfiguration.UseTransport(new LearningTransport { TransportTransactionMode = TransportTransactionMode.ReceiveOnly }).RouteToEndpoint(typeof(MyMessage), "Sample.Receiver");

var persistence = endpointConfiguration.UsePersistence<SqlPersistence>();
persistence.SqlDialect<SqlDialect.MsSqlServer>();
persistence.ConnectionBuilder(() => new SqlConnection(ConnectionString));

#region txsession-nsb-txsessionoptions
var transactionalSessionOptions = new TransactionalSessionOptions { ProcessorEndpoint = "TransactionalSessionProcessor" };
persistence.EnableTransactionalSession(transactionalSessionOptions);
#endregion

endpointConfiguration.EnableOutbox();
endpointConfiguration.SendOnly();

hostBuilder.UseNServiceBus(endpointConfiguration);
#endregion

#region txsession-ef-configuration
// Configure Entity Framework to attach to the synchronized storage session when required
hostBuilder.Services.AddScoped(b =>
{
    if (b.GetService<ISynchronizedStorageSession>() is ISqlStorageSession { Connection: not null } session)
    {
        var context = new MyDataContext(new DbContextOptionsBuilder<MyDataContext>()
            .UseSqlServer(session.Connection)
            .Options);

        //Use the same underlying ADO.NET transaction
        context.Database.UseTransaction(session.Transaction);

        //Ensure context is flushed before the transaction is committed
        session.OnSaveChanges((s, cancellationToken) => context.SaveChangesAsync(cancellationToken));

        return context;
    }
    else
    {
        var context = new MyDataContext(new DbContextOptionsBuilder<MyDataContext>()
            .UseSqlServer(ConnectionString)
            .Options);
        return context;
    }
});
#endregion

#region txsession-web-configuration
hostBuilder.Services.AddScoped<MessageSessionFilter>();
hostBuilder.Services.AddControllers(o => o.Filters.AddService<MessageSessionFilter>());
#endregion

#region txsession-web-configuration-attribute
hostBuilder.Services.AddScoped<ServiceUsingTransactionalSession>();
hostBuilder.Services.AddScoped<RequiresTransactionalSessionAttribute>();
#endregion

var webBuilder = hostBuilder.Build();

webBuilder.UseRouting();
webBuilder.MapControllers();

await webBuilder.RunAsync();