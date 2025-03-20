using System;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using NServiceBus;
using Receiver;

// for SqlExpress use Data Source=.\SqlExpress;Initial Catalog=NsbSamplesSqlNativeIntegration;Integrated Security=True;Max Pool Size=100;Encrypt=false
var connectionString = @"Server=localhost,1433;Initial Catalog=NsbSamplesSqlNativeIntegration;User Id=SA;Password=yourStrong(!)Password;Max Pool Size=100;Encrypt=false";

Console.Title = "NativeIntegration";
var builder = Host.CreateApplicationBuilder(args);

builder.Services.AddSingleton(provider => new InputLoopService(connectionString));
builder.Services.AddHostedService(sp => sp.GetRequiredService<InputLoopService>());

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

Console.WriteLine("Press enter to send a message");
Console.WriteLine("Press any key to exit");


builder.UseNServiceBus(endpointConfiguration);

await builder.Build().RunAsync();


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