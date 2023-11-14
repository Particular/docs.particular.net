using NServiceBus;

#region ApplicationStart
var builder = WebApplication.CreateBuilder(args);

builder.UseNServiceBus(() =>
{
    var endpointConfiguration = new EndpointConfiguration("Samples.Mvc.WebApplication");
    endpointConfiguration.MakeInstanceUniquelyAddressable("1");
    endpointConfiguration.EnableCallbacks();

    endpointConfiguration.UseTransport(new LearningTransport());
    endpointConfiguration.UseSerialization<SystemJsonSerializer>();

    return endpointConfiguration;
});
#endregion

builder.Services.AddMvc();

builder.Services.AddControllersWithViews();

var app = builder.Build();

app.MapControllerRoute("default", "{controller=Home}/{action=SendLinks}/{id?}");

app.Run();
