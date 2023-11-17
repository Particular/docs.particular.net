using NServiceBus;

var builder = WebApplication.CreateBuilder(args);

builder.UseNServiceBus(() =>
{
    var endpointConfiguration = new EndpointConfiguration("Samples.Callbacks.WebSender");
    endpointConfiguration.UseSerialization<SystemJsonSerializer>();
    endpointConfiguration.UseTransport<LearningTransport>();

    endpointConfiguration.MakeInstanceUniquelyAddressable("1");
    endpointConfiguration.EnableCallbacks();

    return endpointConfiguration;
});

builder.Services.AddControllersWithViews();

var app = builder.Build();

app.UseStaticFiles();

app.UseRouting();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
