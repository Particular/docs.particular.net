using Infrastructure;
using Microsoft.AspNetCore.Mvc;
using Scalar.AspNetCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddOpenApi();

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
}

app.UseHttpsRedirection();

app.MapGet("/publish", async ([FromServices] ILogger logger, [FromServices] IMessageSession messageSession) =>
{
    var now = DateTime.UtcNow.ToString();
    await messageSession.Publish(new SomeEventThatIsBeingPublished { SomeValue = now, SomeOtherValue = now });

    return Results.Ok($"Published event at {now}");
});

app.Run();