using System;
using Microsoft.Extensions.Hosting;
using NServiceBus;

//for local instance or SqlExpress
// const string ConnectionString = @"Data Source=(localdb)\mssqllocaldb;Database=nservicebus;Trusted_Connection=True;MultipleActiveResultSets=true";
const string connectionString = @"Server=localhost,1433;Initial Catalog=NsbSamplesSqlMultiInstanceReceiver;User Id=SA;Password=yourStrong(!)Password;Max Pool Size=100;Encrypt=false";

Console.Title = "MultiInstanceReceiver";

#region ReceiverConfiguration

var endpointConfiguration = new EndpointConfiguration("Samples.SqlServer.MultiInstanceReceiver");
endpointConfiguration.UseTransport(new SqlServerTransport(connectionString));

endpointConfiguration.UseSerialization<SystemJsonSerializer>();
endpointConfiguration.EnableInstallers();

#endregion

SqlHelper.EnsureDatabaseExists(connectionString);

var builder = Host.CreateApplicationBuilder();
builder.UseNServiceBus(endpointConfiguration);

var host = builder.Build();
await host.StartAsync();

Console.WriteLine("Press any key to exit");
Console.WriteLine("Waiting for Order messages from the Sender");
Console.ReadKey();

await host.StopAsync();