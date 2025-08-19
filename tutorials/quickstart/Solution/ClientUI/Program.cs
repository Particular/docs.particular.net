using System;
using Messages;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using NServiceBus;

Console.Title = "ClientUI";

var builder = WebApplication.CreateBuilder(args);

var endpointConfiguration = new EndpointConfiguration("ClientUI");
endpointConfiguration.UseSerialization<SystemJsonSerializer>();
var transport = endpointConfiguration.UseTransport<LearningTransport>();

var routing = transport.Routing();
routing.RouteToEndpoint(typeof(PlaceOrder), "Sales");

endpointConfiguration.SendFailedMessagesTo("error");
endpointConfiguration.AuditProcessedMessagesTo("audit");
endpointConfiguration.SendHeartbeatTo("Particular.ServiceControl");

var metrics = endpointConfiguration.EnableMetrics();
metrics.SendMetricDataToServiceControl("Particular.Monitoring", TimeSpan.FromMilliseconds(500));

builder.UseNServiceBus(endpointConfiguration);

builder.Services.AddControllers();
builder.Services.AddMvc();

var app = builder.Build();

app.UseStaticFiles();
app.UseRouting();
app.UseAuthorization();
app.MapControllers();

app.Run();
