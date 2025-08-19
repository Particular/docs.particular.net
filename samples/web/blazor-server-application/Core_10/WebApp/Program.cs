#region ApplicationStart

var builder = WebApplication.CreateBuilder();

var endpointConfiguration = new EndpointConfiguration("Samples.Blazor.WebApplication");
endpointConfiguration.MakeInstanceUniquelyAddressable("1");
endpointConfiguration.EnableCallbacks();
endpointConfiguration.UseSerialization<SystemJsonSerializer>();
endpointConfiguration.UseTransport(new LearningTransport());

builder.UseNServiceBus(endpointConfiguration);

#endregion

builder.Services.AddRazorPages();
builder.Services.AddServerSideBlazor();

var app = builder.Build();

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();
app.MapBlazorHub();
app.MapFallbackToPage("/_Host");

app.Run();
