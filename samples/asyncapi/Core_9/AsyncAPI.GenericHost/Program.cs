using Infrastructure;
using Neuroglia.AsyncApi;
using Neuroglia.AsyncApi.Generation;

var builder = Host.CreateApplicationBuilder(args);

Console.Title = "AsyncAPI Generic Host";

#region GenericHostAddNeurogliaAsyncApi
builder.Services.AddAsyncApi();
#endregion

var endpointConfiguration = new EndpointConfiguration("AsyncAPI.GenericHost");
var routing = endpointConfiguration.UseTransport(new LearningTransport());
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
app.Run();
