#region ApplicationStart

using NServiceBus;

var builder = WebApplication.CreateBuilder(args);

builder.Host.UseNServiceBus(_ =>
{
    var endpointConfiguration = new EndpointConfiguration("Samples.Web.WebApplication");
    endpointConfiguration.MakeInstanceUniquelyAddressable("1");
    endpointConfiguration.EnableCallbacks();
    var transport = endpointConfiguration.UseTransport(new LearningTransport());
    transport.RouteToEndpoint(typeof(Command), "Samples.Web.Server");
    return endpointConfiguration;
});

#endregion

builder.Services.AddRazorPages();
builder.Services.AddControllers();
builder.Services.AddServerSideBlazor();

var app = builder.Build();

app.MapRazorPages();
app.MapControllers();
app.MapControllerRoute("default", "{controller=Sample}/{action=Index}/{id?}");
app.MapBlazorHub();

app.Run();