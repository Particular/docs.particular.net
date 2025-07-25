#region ApplicationStart
var builder = WebApplication.CreateBuilder(args);

var endpointConfiguration = new EndpointConfiguration("Samples.Mvc.WebApplication");
endpointConfiguration.MakeInstanceUniquelyAddressable("1");
endpointConfiguration.EnableCallbacks();

endpointConfiguration.UseTransport(new LearningTransport());
endpointConfiguration.UseSerialization<SystemJsonSerializer>();

builder.UseNServiceBus(endpointConfiguration);
#endregion

builder.Services.AddMvc();

builder.Services.AddControllersWithViews();

var app = builder.Build();

app.MapControllerRoute("default", "{controller=Home}/{action=SendLinks}/{id?}");

app.Run();
