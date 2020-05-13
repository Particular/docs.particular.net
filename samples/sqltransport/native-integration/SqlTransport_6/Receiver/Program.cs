using System;
using System.Data;
using Microsoft.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using NServiceBus;

class Program
{
    static string connectionString = @"Server=127.0.0.1,1401;Database=nservicebustests;User Id=sa;Password=yourStrong(!)Password";

    static async Task Main()
    {
        Console.Title = "Samples.SqlServer.NativeIntegration";

        #region EndpointConfiguration
        var endpointConfiguration = new EndpointConfiguration("Samples.SqlServer.NativeIntegration");
        var transport = endpointConfiguration.UseTransport<SqlServerTransport>();
        transport.Transactions(TransportTransactionMode.SendsAtomicWithReceive);
        transport.ConnectionString(connectionString);
        endpointConfiguration.UseSerialization<NewtonsoftSerializer>()
            .Settings(new JsonSerializerSettings
            {
                TypeNameHandling = TypeNameHandling.Auto,
                SerializationBinder = new SkipAssemblyNameForMessageTypesBinder(new[] { typeof(PlaceOrder), typeof(LegacyOrderDetected) })
            });
        #endregion

        endpointConfiguration.UsePersistence<InMemoryPersistence>();
        endpointConfiguration.EnableInstallers();
        endpointConfiguration.SendFailedMessagesTo("error");
        SqlHelper.EnsureDatabaseExists(connectionString);

        var endpointInstance = await Endpoint.Start(endpointConfiguration)
            .ConfigureAwait(false);
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
            await PlaceOrder()
                .ConfigureAwait(false);
        }
        await endpointInstance.Stop()
            .ConfigureAwait(false);
    }

    static async Task PlaceOrder()
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