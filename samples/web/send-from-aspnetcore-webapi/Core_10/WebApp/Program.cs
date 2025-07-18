#region EndpointConfiguration
var builder = WebApplication.CreateBuilder(args);

var endpointConfiguration = new EndpointConfiguration("Samples.ASPNETCore.Sender");
var transport = endpointConfiguration.UseTransport(new LearningTransport());
transport.RouteToEndpoint(
    assembly: typeof(MyMessage).Assembly,
    destination: "Samples.ASPNETCore.Endpoint");

endpointConfiguration.UseSerialization<SystemJsonSerializer>();
endpointConfiguration.SendOnly();

builder.UseNServiceBus(endpointConfiguration);
#endregion

builder.Services.AddControllers();

var app = builder.Build();

app.MapControllers();

app.Run();
