using Microsoft.Data.SqlClient;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using NServiceBus;
using NServiceBus.Persistence;
using NServiceBus.Persistence.Sql;
using NServiceBus.TransactionalSession;

//for local instance or SqlExpress
const string ConnectionString = @"Data Source=(localdb)\mssqllocaldb;Database=nservicebus;Trusted_Connection=True;MultipleActiveResultSets=true";
//const string ConnectionString = @"Server=localhost,1433;Initial Catalog=nservicebus;User Id=SA;Password=yourStrong(!)Password;Encrypt=false";


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
        // Configure Entity Framework to attach to the synchronized storage session when required
        c.AddScoped(b =>
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
    })
#endregion

    .ConfigureWebHostDefaults(c =>
    {
        c.ConfigureServices(s =>
        {
            #region txsession-web-configuration
            s.AddScoped<MessageSessionFilter>();
            s.AddControllers(o => o.Filters.AddService<MessageSessionFilter>());
            #endregion

            #region txsession-web-configuration-attribute
            s.AddScoped<ServiceUsingTransactionalSession>();
            s.AddScoped<RequiresTransactionalSessionAttribute>();
            #endregion
        });
        c.Configure(app =>
        {
            app.UseRouting();
            app.UseEndpoints(r => r.MapControllers());
        });
    })
    .Build();

await host.RunAsync();