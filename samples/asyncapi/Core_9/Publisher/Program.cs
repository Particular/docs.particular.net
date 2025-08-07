using Infrastructure;
using Microsoft.AspNetCore.Hosting.Server;
using Microsoft.AspNetCore.Mvc;
using Neuroglia.AsyncApi;
using Neuroglia.AsyncApi.Generation;
using Neuroglia.AsyncApi.v3;
using Neuroglia.Data.Schemas.Json;
using Publisher;
using Scalar.AspNetCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddOpenApi();
builder.Services.AddRazorPages();

builder.Services.AddSingleton<IJsonSchemaResolver, JsonSchemaResolver>();
builder.Services.AddHttpClient();

builder.Services.AddTransient<IAsyncApiDocumentGenerator, ApiDocumentGenerator>();

//var generator = builder.GetRequiredService<IAsyncApiDocumentGenerator>();
//var options = new AsyncApiDocumentGenerationOptions()
//{
//    V3BuilderSetup = asyncApi =>
//    {
//        //Setup V3 documents, by configuring servers, for example
//        asyncApi.WithServer("mosquitto", setup =>
//        {
//            setup
//                .WithProtocol("http")
//                .WithHost("http://mosquitto");
//        });
//    }
//};
//IEnumerable<V3AsyncApiDocument> documents = generator.GenerateAsync(typeof(PublisherService), options);

builder.Services.AddAsyncApiGeneration(builder =>
    builder
        //.WithMarkupType<PublisherService>()
        .UseDefaultV3DocumentConfiguration(asyncApi =>
        {
            //Setup V3 documents, by configuring servers, for example
            asyncApi.WithTitle("Publisher Service");
            asyncApi.WithVersion("1.0.0");            
            asyncApi.WithLicense("Apache 2.0", new Uri("https://www.apache.org/licenses/LICENSE-2.0"));           

            asyncApi.WithServer("amqp", setup =>
            {
                setup
                    .WithProtocol(AsyncApiProtocol.Amqp)
                    .WithHost("sb://example.servicebus.windows.net/")
                    .WithBinding(new Neuroglia.AsyncApi.Bindings.Amqp.AmqpServerBindingDefinition
                    {
                        BindingVersion = "0.1.0",
                    });
            });
        }));


builder.Services.AddAsyncApiUI();

var endpointConfiguration = new EndpointConfiguration("Publisher");
endpointConfiguration.UseTransport<LearningTransport>();
endpointConfiguration.UseSerialization<SystemJsonSerializer>();
endpointConfiguration.SendOnly();
endpointConfiguration.EnableAsyncApiSupport();

builder.UseNServiceBus(endpointConfiguration);

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi(); // See - /openapi/v1.json
    app.MapScalarApiReference(); // See - /scalar
    app.MapAsyncApiDocuments();
}

app.UseStaticFiles();
app.UseRouting();
app.UseAuthorization();
app.MapRazorPages();

app.UseHttpsRedirection();

app.MapGet("/publish", async ([FromServices] ILogger logger, [FromServices] IMessageSession messageSession) =>
{
    var now = DateTime.UtcNow.ToString();
    await messageSession.Publish(new SomeEventThatIsBeingPublished { SomeValue = now, SomeOtherValue = now });

    return Results.Ok($"Published event at {now}");
});

app.Run();