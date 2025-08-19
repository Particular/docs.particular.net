#region ApplicationStart
var builder = WebApplication.CreateBuilder(args);

var endpointConfiguration = new EndpointConfiguration("Samples.AsyncPages.WebApplication");
endpointConfiguration.MakeInstanceUniquelyAddressable("1");
endpointConfiguration.EnableCallbacks();
endpointConfiguration.UseTransport(new LearningTransport());
endpointConfiguration.UseSerialization<SystemJsonSerializer>();

builder.UseNServiceBus(endpointConfiguration);
#endregion

builder.Services.AddRazorPages();

var app = builder.Build();

app.MapRazorPages();

app.Run();