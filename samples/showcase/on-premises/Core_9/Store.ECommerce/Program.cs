using NServiceBus;
using Store.Messages.Commands;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllersWithViews();
builder.Services.AddSignalR(c => c.EnableDetailedErrors = true);

builder.UseNServiceBus(() =>
{
    var endpointConfiguration = new EndpointConfiguration("Store.ECommerce");
    endpointConfiguration.PurgeOnStartup(true);
    endpointConfiguration.ApplyCommonConfiguration(routing =>
    {
        routing.RouteToEndpoint(typeof(SubmitOrder).Assembly, "Store.Messages.Commands", "Store.Sales");
    });

    return endpointConfiguration;
});

var app = builder.Build();

app.UseStaticFiles();
app.UseRouting();

app.MapHub<OrdersHub>("/ordershub");
app.MapControllers();
app.MapControllerRoute("default", "{controller=Home}/{action=Index}/{id?}");

app.Run();
