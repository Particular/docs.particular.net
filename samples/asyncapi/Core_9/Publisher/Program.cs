using Infrastructure;
using Microsoft.AspNetCore.Mvc;
using Neuroglia.AsyncApi;
using Neuroglia.Data.Schemas.Json;
using Publisher;
using Scalar.AspNetCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddOpenApi();
builder.Services.AddRazorPages();
builder.Services.AddControllers();

builder.Services.AddSingleton<IJsonSchemaResolver, JsonSchemaResolver>();
builder.Services.AddHttpClient();

builder.Services.AddAsyncApiGeneration(builder =>
    builder.WithMarkupType<PublisherService>()
    .UseDefaultV3DocumentConfiguration(asyncApi =>
    {
        asyncApi.WithServer("mosquitto", setup =>
        {
            setup
                .WithProtocol("http")
                .WithHost("http://mosquitto");
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
app.MapControllers();

app.UseHttpsRedirection();

app.Run();