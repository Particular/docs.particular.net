using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using NServiceBus;
using NServiceBus.Persistence.Sql;
using System;
using System.Linq;
using Microsoft.Data.SqlClient;
using System.Threading.Tasks;
using Microsoft.Extensions.Hosting;

var builder = Host.CreateApplicationBuilder(args);

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

builder.UseNServiceBus(endpointConfiguration);

var host = builder.Build();
await host.StartAsync();
var messageSession = host.Services.GetRequiredService<IMessageSession>();

var random = new Random();
const string letters = "ABCDEFGHIJKLMNOPQRSTUVXYZ";
var locations = new[] { "London", "Paris", "Oslo", "Madrid" };

while (true)
{
    Console.WriteLine("Press enter to send a message, any other key to exit");
    var key = Console.ReadKey();
    Console.WriteLine();

    if (key.Key != ConsoleKey.Enter)
    {
        break;
    }
    var orderId = new string(Enumerable.Range(0, 4).Select(x => letters[random.Next(letters.Length)]).ToArray());
    var shipTo = locations[random.Next(locations.Length)];
    var orderSubmitted = new OrderSubmitted
    {
        OrderId = orderId,
        Value = random.Next(100),
        ShipTo = shipTo
    };
    await messageSession.SendLocal(orderSubmitted);
}

await host.StopAsync();