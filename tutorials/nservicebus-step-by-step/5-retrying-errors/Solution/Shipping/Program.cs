using Microsoft.Extensions.Hosting;

Console.Title = "Shipping";

var builder = Host.CreateApplicationBuilder(args);

var endpointConfiguration = new EndpointConfiguration("Shipping");
endpointConfiguration.UseSerialization<SystemJsonSerializer>();
endpointConfiguration.UseTransport(new LearningTransport());

builder.Services.AddNServiceBus(endpointConfiguration);

var app = builder.Build();

await app.RunAsync();