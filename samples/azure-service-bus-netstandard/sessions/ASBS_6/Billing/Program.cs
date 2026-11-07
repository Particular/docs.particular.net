var builder = Host.CreateApplicationBuilder(args);

builder.Logging.AddConfiguration(builder.Configuration.GetSection("Logging"));
builder.Logging.AddConsole();

var endpointConfiguration = new EndpointConfiguration("Samples.OrderedDelivery.Billing");

var connectionString = Environment.GetEnvironmentVariable("AzureServiceBus_ConnectionString")
                       ?? throw new Exception("Please set up connection string using environment variable 'AzureServiceBus_ConnectionString'");
endpointConfiguration.UseTransport(new AzureServiceBusTransport(connectionString, TopicTopology.Default));

endpointConfiguration.UseSerialization<SystemJsonSerializer>();

// Operational scripting: https://docs.particular.net/transports/azure-service-bus/operational-scripting
endpointConfiguration.EnableInstallers();

builder.Services.AddNServiceBusEndpoint(endpointConfiguration);

var host = builder.Build();

await host.RunAsync();