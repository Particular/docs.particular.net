using System.Data.SqlClient;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using NServiceBus;
using NServiceBus.Persistence;
using NServiceBus.TransactionalSession;

public class Program
{
    const string ConnectionString = @"Server=(localdb)\MSSQLLocalDB;Integrated Security=True;";

    public static void Main()
    {
        using (var myDataContext = new MyDataContext(new DbContextOptionsBuilder<MyDataContext>()
                   .UseSqlServer(new SqlConnection(ConnectionString))
                   .Options))
        {
            myDataContext.Database.EnsureCreated();
        }

        var host = Host.CreateDefaultBuilder()

            #region txsession-nsb-configuration
            .UseNServiceBus(context =>
            {
                var endpointConfiguration = new EndpointConfiguration("Samples.ASPNETCore.Sender");
                var transport = endpointConfiguration.UseTransport(new LearningTransport { TransportTransactionMode = TransportTransactionMode.ReceiveOnly });
                endpointConfiguration.EnableInstallers();

                var persistence = endpointConfiguration.UsePersistence<SqlPersistence>();
                persistence.SqlDialect<SqlDialect.MsSqlServer>();
                persistence.ConnectionBuilder(() => new SqlConnection(ConnectionString));
                persistence.EnableTransactionalSession();

                endpointConfiguration.EnableOutbox();

                return endpointConfiguration;
            })
            #endregion

            #region txsession-ef-configuration
            .ConfigureServices(c =>
            {
                // Configure Entity Framework to attach to the synchronized storage session
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
            #endregion

            .ConfigureWebHostDefaults(c =>
            {
                c.ConfigureServices(s => s.AddControllers());
                c.Configure(app =>
                {
                    #region txsession-web-configuration
                    app.UseMiddleware<MessageSessionMiddleware>();
                    #endregion
                    app.UseRouting();
                    app.UseEndpoints(r => r.MapControllers());
                });
            })
            .Build();

        host.Run();
    }
}
