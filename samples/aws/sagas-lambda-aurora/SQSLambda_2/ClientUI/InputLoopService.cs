using Microsoft.Extensions.Hosting;
public class InputLoopService(IMessageSession messageSession) : BackgroundService
{
    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        Console.WriteLine("Press [Enter] to place an order. Press [Esc] to quit.");

        while (true)
        {
            var pressedKey = Console.ReadKey(true);

            switch (pressedKey.Key)
            {
                case ConsoleKey.Enter:
                    {
                        var orderId = Guid.NewGuid().ToString("N");
                        await messageSession.Send(new PlaceOrder() { OrderId = orderId });
                        Console.WriteLine($"Order {orderId} was placed.");

                        break;
                    }
                case ConsoleKey.Escape:
                    {
                        return;
                    }
            }
        }
    }
}
