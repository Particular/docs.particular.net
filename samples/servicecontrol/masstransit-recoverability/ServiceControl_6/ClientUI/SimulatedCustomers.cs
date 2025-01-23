namespace ClientUI;

using MassTransit;
using Messages;
using Microsoft.Extensions.Hosting;

class SimulatedCustomers(IBus _bus) : BackgroundService
{
    public void WriteState(TextWriter output)
    {
        output.WriteLine($"Sending {rate} orders / second");
    }

    public void IncreaseTraffic()
    {
        rate++;
    }

    public void DecreaseTraffic()
    {
        if (rate > 0)
        {
            rate--;
        }
    }

    Task PlaceSingleOrder(CancellationToken cancellationToken)
    {
        var placeOrderCommand = new PlaceOrder { OrderId = Guid.NewGuid().ToString() };

        Console.Write("!");
        return _bus.Publish(placeOrderCommand, cancellationToken);
    }

    protected override async Task ExecuteAsync(CancellationToken cancellationToken = default)
    {
        while (!cancellationToken.IsCancellationRequested)
        {
            try
            {
                await Task.WhenAll(
                    SendBatch(cancellationToken),
                    Task.Delay(1000, cancellationToken)
                );

            }
            catch (TaskCanceledException)
            {
                break;
            }
        }
    }

    async Task SendBatch(CancellationToken cancellationToken)
    {
        var x = rate;
        if (rate > 0)
        {
            var tasks = new List<Task>(x);

            for (int i = 0; i < x; i++)
            {
                tasks.Add(PlaceSingleOrder(cancellationToken));
            }

            await Task.WhenAll(tasks);
        }
    }

    int rate = 1;
}