using NServiceBus;
using System.Data.SqlClient;
using Shared;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Host.UseNServiceBus(context =>
{
    var config = new EndpointConfiguration("KubernetesDemo.Publisher");
    config.UseSerialization<SystemJsonSerializer>();

    config.EnableInstallers();

    var transport = new LearningTransport
    {
        StorageDirectory = "/transport"
    };
    config.UseTransport(transport);

    return config;
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.MapGet("/publish", async (IMessageSession messageSession) =>
{
    Console.WriteLine("Publishing message!");
    await messageSession.Publish(new DemoEvent(){ Id = Guid.NewGuid().ToString() });
    return "message published";
}).WithOpenApi();


app.Run();