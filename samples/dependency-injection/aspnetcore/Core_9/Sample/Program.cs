var builder = WebApplication.CreateBuilder(args);

#region ContainerConfiguration
builder.Services.AddSingleton<MyService>();
builder.UseNServiceBus(() =>
{
    var endpointConfiguration = new EndpointConfiguration("Sample.Core");
    endpointConfiguration.UseSerialization<SystemJsonSerializer>();
    endpointConfiguration.UseTransport<LearningTransport>();
    return endpointConfiguration;
});
#endregion

var app = builder.Build();

#region RequestHandling
app.MapGet("/", context =>
{
    var endpointInstance = context.RequestServices.GetService<IMessageSession>();
    var myMessage = new MyMessage();

    return Task.WhenAll(
        endpointInstance.SendLocal(myMessage),
        context.Response.WriteAsync("Message sent"));
});
#endregion

app.Run();
