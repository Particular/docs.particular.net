using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using NServiceBus;

var builder = WebApplication.CreateBuilder();

builder.Services.AddRazorPages();

#region ApplicationStart
builder.Host.UseNServiceBus(context =>
{
    var endpointConfiguration = new EndpointConfiguration("Samples.AsyncPages.WebApplication");
    endpointConfiguration.MakeInstanceUniquelyAddressable("1");
    endpointConfiguration.EnableCallbacks();
    endpointConfiguration.UseTransport<LearningTransport>();
    return endpointConfiguration;
});
#endregion

var app = builder.Build();

app.MapRazorPages();

app.Run();