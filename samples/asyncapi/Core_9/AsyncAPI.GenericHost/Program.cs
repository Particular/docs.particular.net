using Infrastructure;
using Neuroglia.AsyncApi;
using Neuroglia.AsyncApi.Generation;
using Neuroglia.AsyncApi.IO;

var builder = Host.CreateApplicationBuilder(args);

builder.Services.AddWindowsService();

builder.Services.AddAsyncApi();
builder.Services.AddTransient<IAsyncApiDocumentGenerator, ApiDocumentGenerator>();

var endpointConfiguration = new EndpointConfiguration("AsyncAPI.GenericHost");
var routing = endpointConfiguration.UseTransport(new LearningTransport());
endpointConfiguration.UseSerialization<SystemJsonSerializer>();
endpointConfiguration.EnableInstallers();
endpointConfiguration.EnableAsyncApiSupport();


builder.UseNServiceBus(endpointConfiguration);

builder.Services.AddHostedService<AsyncAPISchemaWriter>();
builder.Services.AddHostedService<Worker>();

var app = builder.Build();
app.Run();
