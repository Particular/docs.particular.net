using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using NServiceBus;
using NServiceBus.Persistence.Sql;
using System;
using Microsoft.Data.SqlClient;
using System.Threading.Tasks;
using Microsoft.Extensions.Hosting;

var builder = Host.CreateApplicationBuilder(args);
builder.Services.AddHostedService<Sender>();

//for local instance or SqlExpress
//var connectionString = @"Data Source=(localdb)\mssqllocaldb;Database=NsbSamplesEfCoreUowSql;Trusted_Connection=True;MultipleActiveResultSets=true";

var connectionString = @"Server=localhost,1433;Initial Catalog=NsbSamplesEfCoreUowSql;User Id=SA;Password=yourStrong(!)Password;Encrypt=false;Max Pool Size=100;TrustServerCertificate=True";
Console.Title = "EntityFrameworkUnitOfWork";

using (var receiverDataContext = new ReceiverDataContext(new DbContextOptionsBuilder<ReceiverDataContext>()
    .UseSqlServer(connectionString)
    .Options))
{
    await receiverDataContext.Database.EnsureCreatedAsync();
}

var endpointConfiguration = new EndpointConfiguration("Samples.EntityFrameworkUnitOfWork.SQL");
endpointConfiguration.EnableInstallers();
endpointConfiguration.ExecuteTheseHandlersFirst(typeof(CreateOrderHandler), typeof(OrderLifecycleSaga), typeof(CreateShipmentHandler));

endpointConfiguration.UseTransport(new SqlServerTransport(connectionString)
{
    Subscriptions = { DisableCaching = true },
    TransportTransactionMode = TransportTransactionMode.SendsAtomicWithReceive
});

endpointConfiguration.UseSerialization<SystemJsonSerializer>();

var persistence = endpointConfiguration.UsePersistence<SqlPersistence>();
persistence.ConnectionBuilder(() => new SqlConnection(connectionString));
var dialect = persistence.SqlDialect<SqlDialect.MsSqlServer>();

#region UnitOfWork_SQL

endpointConfiguration.RegisterComponents(c =>
{
    c.AddScoped(b =>
    {
        var session = b.GetRequiredService<ISqlStorageSession>();

        var context = new ReceiverDataContext(new DbContextOptionsBuilder<ReceiverDataContext>()
            .UseSqlServer(session.Connection)
            .Options);

        //Use the same underlying ADO.NET transaction
        context.Database.UseTransaction(session.Transaction);

        //Ensure context is flushed before the transaction is committed
        session.OnSaveChanges((s, token) => context.SaveChangesAsync(token));

        return context;
    });
});

#endregion

Console.WriteLine("Press any key, the application is starting");
Console.ReadKey();
Console.WriteLine("Starting...");

builder.UseNServiceBus(endpointConfiguration);

await builder.Build().RunAsync();
