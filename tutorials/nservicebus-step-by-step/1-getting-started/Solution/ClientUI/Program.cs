using Microsoft.Extensions.Hosting;

Console.Title = "ClientUI";

var builder = Host.CreateApplicationBuilder(args);

var endpointConfiguration = new EndpointConfiguration("ClientUI");
endpointConfiguration.UseSerialization<SystemJsonSerializer>();
endpointConfiguration.UseTransport(new LearningTransport());

builder.Services.AddNServiceBusEndpoint(endpointConfiguration);

var app = builder.Build();

await app.RunAsync();