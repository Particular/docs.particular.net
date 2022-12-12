using NServiceBus;

#region ApplicationStart

var builder = WebApplication.CreateBuilder(args);

builder.Host.UseNServiceBus(context =>
{
    var endpointConfiguration = new EndpointConfiguration("Samples.Blazor.WebApplication");
    endpointConfiguration.MakeInstanceUniquelyAddressable("1");
    endpointConfiguration.EnableCallbacks();
    endpointConfiguration.UseTransport(new LearningTransport());
    return endpointConfiguration;
});

#endregion

builder.Services.AddRazorPages();
builder.Services.AddServerSideBlazor();

var app = builder.Build();

if (!app.Environment.IsDevelopment())
{
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();

app.UseStaticFiles();

app.UseRouting();

app.MapBlazorHub();
app.MapFallbackToPage("/_Host");

app.Run();
