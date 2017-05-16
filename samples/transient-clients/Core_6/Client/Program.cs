using System;
using System.Threading.Tasks;
using Microsoft.AspNet.SignalR.Client;

static class Program
{
    static void Main()
    {
        AsyncMain().GetAwaiter().GetResult();
    }

    static async Task AsyncMain()
    {
        Console.Title = "Samples.TransientClients.Client";

        var url = "http://localhost:8080";

        var hubConnection = new HubConnection(url);
        var stockHubProxy = hubConnection.CreateHubProxy("StockTicksHub");
        stockHubProxy.On<StockTick>("StockTick", stock => Console.WriteLine($"Stock update for {stock.Symbol} at {stock.Timestamp:O}. Press any key to exit."));
        await hubConnection.Start();

        Console.WriteLine("Press any key to exit.");
        Console.ReadKey(true);

        hubConnection.Stop();
    }
}
