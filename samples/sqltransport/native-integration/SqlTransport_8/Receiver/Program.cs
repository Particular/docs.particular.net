using System;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using NServiceBus;

// for SqlExpress use Data Source=.\SqlExpress;Initial Catalog=NsbSamplesSqlNativeIntegration;Integrated Security=True;Max Pool Size=100;Encrypt=false
//var connectionString = @"Server=localhost,1433;Initial Catalog=NsbSamplesSqlNativeIntegration;User Id=SA;Password=yourStrong(!)Password;Max Pool Size=100;Encrypt=false";
// for LocalDB
var connectionString = @"Data Source=(localdb)\MSSQLLocalDB;Initial Catalog=NsbSamplesSqlNativeIntegration;Integrated Security=True;Connect Timeout=30;Encrypt=False;Max Pool Size=100;Trust Server Certificate=False;Application Intent=ReadWrite;Multi Subnet Failover=False";

Console.Title = "NativeIntegration";
var builder = Host.CreateApplicationBuilder(args);

#region EndpointConfiguration

var endpointConfiguration = new EndpointConfiguration("Samples.SqlServer.NativeIntegration");
endpointConfiguration.UseTransport(new SqlServerTransport(connectionString)
{
    TransportTransactionMode = TransportTransactionMode.SendsAtomicWithReceive
});
endpointConfiguration.UseSerialization<NewtonsoftJsonSerializer>()
    .Settings(new JsonSerializerSettings
    {
        TypeNameHandling = TypeNameHandling.Auto,
        SerializationBinder = new SkipAssemblyNameForMessageTypesBinder([typeof(PlaceOrder), typeof(LegacyOrderDetected)])
    });

#endregion

endpointConfiguration.EnableInstallers();

await SqlHelper.EnsureDatabaseExists(connectionString);

builder.UseNServiceBus(endpointConfiguration);

var host = builder.Build();

await host.StartAsync();

// Get the application stopping token to handle graceful shutdown
var ct = host.Services.GetRequiredService<IHostApplicationLifetime>().ApplicationStopping;

Console.WriteLine("Press [Enter] to send a message, or [CTRL+C] to exit.");

while (!ct.IsCancellationRequested)
{
    if (!Console.KeyAvailable)
    {
        // Wait a short time before checking again
        await Task.Delay(100, CancellationToken.None);
        continue;
    }

    var input = Console.ReadKey();
    Console.WriteLine();

    if (input.Key == ConsoleKey.Enter)
    {
        await PlaceOrder(connectionString, ct);
    }
}

await host.StopAsync();

static async Task PlaceOrder(string connectionString, CancellationToken ct)
{
    #region MessagePayload

    var message = @"{
                           $type: 'PlaceOrder, Receiver',
                           OrderId: 'Order from ADO.net sender'
                        }";

    #endregion

    #region SendingUsingAdoNet

    var insertSql = @"insert into [Samples.SqlServer.NativeIntegration]
                                      (Id, Recoverable, Headers, Body)
                               values (@Id, @Recoverable, @Headers, @Body)";
    using (var connection = new SqlConnection(connectionString))
    {
        await connection.OpenAsync(ct);

        using (var command = new SqlCommand(insertSql, connection))
        {
            var parameters = command.Parameters;
            parameters.Add("Id", SqlDbType.UniqueIdentifier).Value = Guid.NewGuid();
            parameters.Add("Headers", SqlDbType.NVarChar).Value = "";
            var body = Encoding.UTF8.GetBytes(message);
            parameters.Add("Body", SqlDbType.VarBinary).Value = body;
            parameters.Add("Recoverable", SqlDbType.Bit).Value = true;

            await command.ExecuteNonQueryAsync(ct);
        }
    }

    #endregion
}

class SkipAssemblyNameForMessageTypesBinder(Type[] messageTypes) : ISerializationBinder
{
    public Type BindToType(string assemblyName, string typeName)
    {
        return messageTypes.FirstOrDefault(messageType => messageType.FullName == typeName);
    }

    public void BindToName(Type serializedType, out string assemblyName, out string typeName)
    {
        assemblyName = serializedType.Assembly.FullName;
        typeName = serializedType.FullName;
    }
}