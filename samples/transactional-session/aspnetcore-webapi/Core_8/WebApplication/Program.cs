using System.Data.SqlClient;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using NServiceBus;
using NServiceBus.Persistence;

public class Program
{
    public static void Main()
    {
        var connectionString = @"Server=(localdb)\MSSQLLocalDB;Integrated Security=True;";

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
                var transport = endpointConfiguration.UseTransport(new LearningTransport { TransportTransactionMode = TransportTransactionMode.ReceiveOnly });
                endpointConfiguration.EnableInstallers();

                var persistence = endpointConfiguration.UsePersistence<SqlPersistence>();
                persistence.SqlDialect<SqlDialect.MsSqlServer>();
                persistence.ConnectionBuilder(() => new SqlConnection(connectionString));

                endpointConfiguration.EnableOutbox();
                endpointConfiguration.EnableTransactionalSession();


                return endpointConfiguration;
            })
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
            .ConfigureWebHostDefaults(c =>
            {
                c.ConfigureServices(s => s.AddControllers());
                c.Configure(app =>
                {
                    app.UseDeveloperExceptionPage();
                    app.UseMiddleware<MessageSessionMiddleware>();
                    app.UseRouting();
                    app.UseEndpoints(r => r.MapControllers());
                    app.UseStatusCodePages();
                });
            })
            .Build();
        #endregion

        host.Run();
    }
}
