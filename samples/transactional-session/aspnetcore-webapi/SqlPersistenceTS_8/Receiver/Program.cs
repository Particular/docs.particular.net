using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

Console.Title = "Receiver";

var builder = Host.CreateApplicationBuilder(args);

// for SqlExpress use Data Source=.\SqlExpress;Initial Catalog=nservicebus;Integrated Security=True;Encrypt=false
const string ConnectionString = @"Server=localhost,1433;Initial Catalog=nservicebus;User Id=SA;Password=yourStrong(!)Password;Encrypt=false";

builder.Services.AddDbContext<MyDataContext>(o => o.UseSqlServer(new SqlConnection(ConnectionString)));

var endpointConfiguration = new EndpointConfiguration("Sample.Receiver");

endpointConfiguration.UseSerialization<SystemJsonSerializer>();

endpointConfiguration.EnableInstallers();

endpointConfiguration.UseTransport(new LearningTransport { TransportTransactionMode = TransportTransactionMode.ReceiveOnly });

builder.UseNServiceBus(endpointConfiguration);

await builder.Build().RunAsync();