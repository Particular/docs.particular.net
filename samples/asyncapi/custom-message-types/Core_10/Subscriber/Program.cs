using AsyncAPI.Feature;
using Microsoft.Extensions.Hosting;

Console.Title = "AsyncAPI Subscriber";

var builder = Host.CreateApplicationBuilder(args);

var endpointConfiguration = new EndpointConfiguration("Subscriber");
endpointConfiguration.UseTransport<LearningTransport>();
endpointConfiguration.UseSerialization<SystemJsonSerializer>();
endpointConfiguration.EnableInstallers();

#region SubscriberEnableAsyncAPIOnNSB 
endpointConfiguration.EnableAsyncApiSupport();
#endregion

builder.Services.AddNServiceBusEndpoint(endpointConfiguration);

var app = builder.Build();
await app.RunAsync();