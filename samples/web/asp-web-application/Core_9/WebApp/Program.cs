#region ApplicationStart
var builder = WebApplication.CreateBuilder(args);

var endpointConfiguration = new EndpointConfiguration("Samples.Web.WebApplication");
endpointConfiguration.MakeInstanceUniquelyAddressable("1");
endpointConfiguration.EnableCallbacks();

var transport = endpointConfiguration.UseTransport(new LearningTransport());
transport.RouteToEndpoint(typeof(Command), "Samples.Web.Server");

endpointConfiguration.UseSerialization<SystemJsonSerializer>();

builder.UseNServiceBus(endpointConfiguration);
#endregion

builder.Services.AddRazorPages();

builder.Services.AddControllers();

var app = builder.Build();

app.MapRazorPages();
app.MapControllers();
app.MapControllerRoute("default", "{controller=Sample}/{action=Index}/{id?}");

app.Run();