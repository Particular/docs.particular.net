using System;
using Microsoft.Extensions.Hosting;
using NServiceBus;
using Shared;

Console.Title = "Endpoint2";

var builder = Host.CreateApplicationBuilder(args);

var endpointConfiguration = new EndpointConfiguration("Samples.ASBS.SendReply.Endpoint2");
endpointConfiguration.EnableInstallers();

string keyVaultUri = Environment.GetEnvironmentVariable("KeyVaultUri");
string connectionString = await new KeyVaultBasedConfigurationProvider(keyVaultUri).GetConfiguration("AzureServiceBusConnectionString");
if (string.IsNullOrWhiteSpace(connectionString))
{
    throw new Exception("Could not read the 'AzureServiceBusConnectionString' value. Check the sample prerequisites.");
}

var transport = new AzureServiceBusTransport(connectionString);
endpointConfiguration.UseTransport(transport);
endpointConfiguration.UseSerialization<SystemJsonSerializer>();

Console.WriteLine("Press any key, the application is starting");
Console.ReadKey();
Console.WriteLine("Starting...");

builder.UseNServiceBus(endpointConfiguration);
await builder.Build().RunAsync();
