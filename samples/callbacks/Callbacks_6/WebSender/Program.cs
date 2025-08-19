var builder = WebApplication.CreateBuilder(args);

var endpointConfiguration = new EndpointConfiguration("Samples.Callbacks.WebSender");

endpointConfiguration.UseSerialization<SystemJsonSerializer>();
endpointConfiguration.UseTransport<LearningTransport>();

endpointConfiguration.MakeInstanceUniquelyAddressable("1");
endpointConfiguration.EnableCallbacks();

builder.UseNServiceBus(endpointConfiguration);

builder.Services.AddControllersWithViews();

var app = builder.Build();

app.UseStaticFiles();

app.UseRouting();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();