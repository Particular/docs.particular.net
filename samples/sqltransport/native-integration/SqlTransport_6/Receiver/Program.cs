using System;
using System.Data;
using Microsoft.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using NServiceBus;

// for SqlExpress use Data Source=.\SqlExpress;Initial Catalog=NsbSamplesSqlNativeIntegration;Integrated Security=True;Max Pool Size=100;Encrypt=false
var connectionString = @"Server=localhost,1433;Initial Catalog=NsbSamplesSqlNativeIntegration;User Id=SA;Password=yourStrong(!)Password;Max Pool Size=100;Encrypt=false";

Console.Title = "Samples.SqlServer.NativeIntegration";

#region EndpointConfiguration
var endpointConfiguration = new EndpointConfiguration("Samples.SqlServer.NativeIntegration");
var transport = endpointConfiguration.UseTransport<SqlServerTransport>();
transport.Transactions(TransportTransactionMode.SendsAtomicWithReceive);
transport.ConnectionString(connectionString);
endpointConfiguration.UseSerialization<NewtonsoftJsonSerializer>()
    .Settings(new JsonSerializerSettings
    {
        TypeNameHandling = TypeNameHandling.Auto,
        SerializationBinder = new SkipAssemblyNameForMessageTypesBinder(new[] { typeof(PlaceOrder), typeof(LegacyOrderDetected) })
    });
#endregion

endpointConfiguration.EnableInstallers();

await SqlHelper.EnsureDatabaseExists(connectionString);

var endpointInstance = await Endpoint.Start(endpointConfiguration);

Console.WriteLine("Press enter to send a message");
Console.WriteLine("Press any key to exit");

while (true)
{
    var key = Console.ReadKey();
    Console.WriteLine();

    if (key.Key != ConsoleKey.Enter)
    {
        break;
    }

    await PlaceOrder(connectionString);
}

await endpointInstance.Stop();

static async Task PlaceOrder(string connectionString)
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
        await connection.OpenAsync()
            .ConfigureAwait(false);

        using (var command = new SqlCommand(insertSql, connection))
        {
            var parameters = command.Parameters;
            parameters.Add("Id", SqlDbType.UniqueIdentifier).Value = Guid.NewGuid();
            parameters.Add("Headers", SqlDbType.NVarChar).Value = "";
            var body = Encoding.UTF8.GetBytes(message);
            parameters.Add("Body", SqlDbType.VarBinary).Value = body;
            parameters.Add("Recoverable", SqlDbType.Bit).Value = true;

            await command.ExecuteNonQueryAsync()
                .ConfigureAwait(false);
        }
    }

    #endregion
}

class SkipAssemblyNameForMessageTypesBinder : ISerializationBinder
{
    Type[] messageTypes;

    public SkipAssemblyNameForMessageTypesBinder(Type[] messageTypes)
    {
        this.messageTypes = messageTypes;
    }

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