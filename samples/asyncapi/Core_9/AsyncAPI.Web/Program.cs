using AsyncAPI.Feature;
using Microsoft.AspNetCore.Mvc;
using Neuroglia.AsyncApi;
using Neuroglia.AsyncApi.Bindings.Amqp;
using Scalar.AspNetCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddOpenApi();

// Required by the AsyncAPI UI
builder.Services.AddRazorPages();

builder.Services.AddHttpClient();

#region WebAppAddNeurogliaAsyncApi
// Configures code-first AsyncAPI document generation.
builder.Services.AddAsyncApiGeneration(asyncApiBuilder =>
    asyncApiBuilder
        .UseDefaultV3DocumentConfiguration(asyncApi =>
        {
            //Setup V3 documents, by configuring servers, for example:
            asyncApi.WithTitle("Web Service");
            asyncApi.WithVersion("1.0.0");
            asyncApi.WithLicense(
                "Apache 2.0",
                new Uri("https://www.apache.org/licenses/LICENSE-2.0"));

            asyncApi.WithServer("amqp", setup =>
            {
                setup
                    .WithProtocol(AsyncApiProtocol.Amqp)
                    .WithHost("sb://example.servicebus.windows.net/")
                    .WithBinding(new AmqpServerBindingDefinition
                    {
                        BindingVersion = "0.1.0",
                    });
            });
        }));

// Adds AsyncAPI UI services. Available at /asyncapi.
builder.Services.AddAsyncApiUI();
#endregion

var endpointConfiguration = new EndpointConfiguration("AsyncAPI.Web");
endpointConfiguration.UseTransport<LearningTransport>();
endpointConfiguration.UseSerialization<SystemJsonSerializer>();
endpointConfiguration.SendOnly();

#region WebAppEnableAsyncAPIOnNSB
endpointConfiguration.EnableAsyncApiSupport();
#endregion

builder.UseNServiceBus(endpointConfiguration);

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.MapScalarApiReference();

    // Configures AsyncAPI middleware to serve generated documents.
    app.MapAsyncApiDocuments();
}

app.UseStaticFiles();
app.UseRouting();
app.UseAuthorization();
app.MapRazorPages();

app.UseHttpsRedirection();

app.MapGet("/publishfirst", async ([FromServices] ILogger logger, [FromServices] IMessageSession messageSession) =>
{
    var now = DateTime.UtcNow.ToString();
    await messageSession.Publish(new FirstEventThatIsBeingPublished { SomeValue = now, SomeOtherValue = now });

    return Results.Ok($"Published first event at {now}");
});

app.MapGet("/publishsecond", async ([FromServices] ILogger logger, [FromServices] IMessageSession messageSession) =>
{
    var now = DateTime.UtcNow.ToString();
    await messageSession.Publish(new SecondEventThatIsBeingPublished { SomeValue = now, SomeOtherValue = now });

    return Results.Ok($"Published second event at {now}");
});

await app.RunAsync();