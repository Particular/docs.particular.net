using AsyncAPI.Feature;
using Neuroglia.AsyncApi;

var builder = Host.CreateApplicationBuilder(args);

Console.Title = "AsyncAPI Generic Host";

#region GenericHostAddNeurogliaAsyncApi
builder.Services.AddAsyncApi();
#endregion

var endpointConfiguration = new EndpointConfiguration("AsyncAPI.GenericHost");
endpointConfiguration.UseTransport(new LearningTransport());
endpointConfiguration.UseSerialization<SystemJsonSerializer>();
endpointConfiguration.EnableInstallers();

#region GenericHostEnableAsyncAPIOnNSB
endpointConfiguration.EnableAsyncApiSupport();
#endregion

builder.UseNServiceBus(endpointConfiguration);

#region GenericHostAddSchemaWriter
builder.Services.AddHostedService<AsyncAPISchemaWriter>();
#endregion

builder.Services.AddHostedService<Worker>();

var app = builder.Build();
await app.RunAsync();
