using System;
using System.Threading.Tasks;
using NServiceBus;

Console.Title = "NativeIntegration";

#region EndpointName

var endpointConfiguration = new EndpointConfiguration("Samples.ASB.NativeIntegration");

#endregion

endpointConfiguration.EnableInstallers();
endpointConfiguration.UseSerialization<SystemJsonSerializer>();
endpointConfiguration.Conventions().DefiningMessagesAs(type => type.Name == "NativeMessage");


var connectionString = Environment.GetEnvironmentVariable("AzureServiceBus_ConnectionString");
if (string.IsNullOrWhiteSpace(connectionString))
{
    throw new Exception("Could not read the 'AzureServiceBus_ConnectionString' environment variable. Check the sample prerequisites.");
}

var transport = new AzureServiceBusTransport(connectionString);
endpointConfiguration.UseTransport(transport);

var endpointInstance = await Endpoint.Start(endpointConfiguration);
Console.WriteLine("Press any key to exit");
Console.ReadKey();
await endpointInstance.Stop();