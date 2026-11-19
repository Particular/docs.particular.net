var builder = Host.CreateApplicationBuilder(args);

var endpointConfiguration = new EndpointConfiguration("Receiver");

var connectionString = builder.Configuration.GetConnectionString("AzureServiceBusConnectionString") ?? string.Empty;
endpointConfiguration.UseTransport(new AzureServiceBusTransport(connectionString, TopicTopology.Default));
endpointConfiguration.UseSerialization<SystemJsonSerializer>();

endpointConfiguration.AuditProcessedMessagesTo("audit");

// Operational scripting: https://docs.particular.net/transports/azure-service-bus/operational-scripting
endpointConfiguration.EnableInstallers();

builder.Services.AddNServiceBusEndpoint(endpointConfiguration);

var host = builder.Build();

await host.RunAsync();
