using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.SignalR.Client;

static class Program
{
    static async Task Main()
    {
        Console.Title = "Client";

        Console.WriteLine("Press any key to connect.");
        Console.ReadKey(true);

        var hubConnection = new HubConnectionBuilder()
            .WithUrl("http://localhost:9756/StockTicksHub")
            .Build();

        hubConnection.On<StockTick>("PushStockTick", stock => Console.WriteLine($"Stock update for {stock.Symbol} at {stock.Timestamp:O}. Press any key to exit."));

        await hubConnection.StartAsync();

        Console.WriteLine("Press any key to exit.");
        Console.ReadKey(true);

        await hubConnection.StopAsync();
    }
}