using Microsoft.AspNetCore.Builder;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.DependencyInjection;
using NServiceBus;
using NServiceBus.TransactionalSession;

const string ConnectionString = @"";

var hostBuilder = WebApplication.CreateBuilder(args);

var endpointConfiguration = new EndpointConfiguration("Samples.ASPNETCore.Sender");

endpointConfiguration.UseSerialization<SystemJsonSerializer>();

endpointConfiguration.SendOnly();

endpointConfiguration.EnableInstallers();

endpointConfiguration.UseTransport(new SqlServerTransport(ConnectionString))
    .RouteToEndpoint(typeof(MyMessage), "Receiver");

var persistence = endpointConfiguration.UsePersistence<SqlPersistence>();
persistence.SqlDialect<SqlDialect.MsSqlServer>();
persistence.ConnectionBuilder(() => new SqlConnection(ConnectionString));

var transactionalSessionOptions = new TransactionalSessionOptions
{
    ProcessorEndpoint = "Processor"
};

persistence.EnableTransactionalSession(transactionalSessionOptions);

endpointConfiguration.EnableOutbox();

hostBuilder.UseNServiceBus(endpointConfiguration);

#region txsession-web-configuration
hostBuilder.Services.AddScoped<MessageSessionFilter>();
hostBuilder.Services.AddControllers(o => o.Filters.AddService<MessageSessionFilter>());
#endregion

var webBuilder = hostBuilder.Build();

webBuilder.UseRouting();
webBuilder.MapControllers();

await webBuilder.RunAsync();