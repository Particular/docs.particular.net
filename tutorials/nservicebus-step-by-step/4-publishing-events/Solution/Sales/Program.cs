using Microsoft.Extensions.Hosting;

Console.Title = "Sales";

var builder = Host.CreateApplicationBuilder(args);

var endpointConfiguration = new EndpointConfiguration("Sales");
endpointConfiguration.UseSerialization<SystemJsonSerializer>();
endpointConfiguration.UseTransport(new LearningTransport());

builder.Services.AddNServiceBus(endpointConfiguration);

var app = builder.Build();

await app.RunAsync();