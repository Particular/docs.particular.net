using ClientHub;

Console.Title = "ClientHub";

var builder = WebApplication.CreateBuilder(args);

var endpointConfiguration = new EndpointConfiguration("Samples.NearRealTimeClients.ClientHub");

endpointConfiguration.UseSerialization<SystemJsonSerializer>();
endpointConfiguration.UseTransport(new LearningTransport());

endpointConfiguration.SendFailedMessagesTo("error");

var conventions = endpointConfiguration.Conventions();
conventions.DefiningEventsAs(type => type == typeof(StockTick));

builder.UseNServiceBus(endpointConfiguration);

builder.Services.AddSignalR();

var app = builder.Build();
app.MapHub<StockTicksHub>("/StockTicksHub");

const string url = "http://localhost:9756/";

var webAppTask = app.RunAsync(url);

Console.WriteLine($"SignalR server running at {url}");
Console.WriteLine("NServiceBus subscriber running");
Console.WriteLine("Press any key to exit");
Console.ReadKey(true);

await Task.WhenAll(
    app.StopAsync(),
    webAppTask);
