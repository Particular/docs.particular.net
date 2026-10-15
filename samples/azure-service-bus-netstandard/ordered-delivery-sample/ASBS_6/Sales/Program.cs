var builder = Host.CreateApplicationBuilder(args);

builder.Logging.AddConfiguration(builder.Configuration.GetSection("Logging"));
builder.Logging.AddConsole();

var endpointConfiguration = new EndpointConfiguration("Samples.OrderedDelivery.Sales");

var connectionString = builder.Configuration.GetConnectionString("AzureServiceBusConnectionString") ?? string.Empty;
endpointConfiguration.UseTransport(new AzureServiceBusTransport(connectionString, TopicTopology.Default)
    { EnableSessions = true });

endpointConfiguration.UseSerialization<SystemJsonSerializer>();

// Operational scripting: https://docs.particular.net/transports/azure-service-bus/operational-scripting
endpointConfiguration.EnableInstallers();

//disable delayed retries for session enabled endpoints
endpointConfiguration.Recoverability().Delayed(d => d.NumberOfRetries(0));

builder.Services.AddNServiceBusEndpoint(endpointConfiguration);

var host = builder.Build();

await host.RunAsync();