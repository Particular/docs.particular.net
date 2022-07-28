using System.Data.SqlClient;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using NServiceBus;
using NServiceBus.Persistence;
using NServiceBus.Persistence.Sql;

public class Program
{
    public static void Main()
    {
        var connectionString = @"server=(local);database=nservicebus;Integrated Security=True;Max Pool Size=100";

        using (var myDataContext = new MyDataContext(new DbContextOptionsBuilder<MyDataContext>()
                   .UseSqlServer(new SqlConnection(connectionString))
                   .Options))
        {
            myDataContext.Database.EnsureCreated();
        }

        #region EndpointConfiguration
        var host = Host.CreateDefaultBuilder()
            .UseNServiceBus(context =>
            {
                var endpointConfiguration = new EndpointConfiguration("Samples.ASPNETCore.Sender");
                var transport = endpointConfiguration.UseTransport(new LearningTransport());
                transport.RouteToEndpoint(
                    assembly: typeof(MyMessage).Assembly,
                    destination: "Samples.ASPNETCore.Endpoint");

                var persistence = endpointConfiguration.UsePersistence<SqlPersistence>();
                persistence.SqlDialect<SqlDialect.MsSqlServer>();
                persistence.ConnectionBuilder(() => new SqlConnection(connectionString));
                endpointConfiguration.EnableTransactionalSession();

                    

                return endpointConfiguration;
            })
            .ConfigureServices(c =>
            {
                c.AddScoped(b =>
                {
                    var session = b.GetRequiredService<ISynchronizedStorageSession>();
                    var context = new MyDataContext(new DbContextOptionsBuilder<MyDataContext>()
                        .UseSqlServer(session.SqlPersistenceSession().Connection)
                        .Options);

                    //Use the same underlying ADO.NET transaction
                    context.Database.UseTransaction(session.SqlPersistenceSession().Transaction);

                    //Ensure context is flushed before the transaction is committed
                    session.SqlPersistenceSession().OnSaveChanges((s, token) => context.SaveChangesAsync(token));

                    return context;
                });
            })
            .ConfigureWebHostDefaults(c => c.UseStartup<Startup>())
            .Build();
        #endregion

        host.Run();
    }
}
