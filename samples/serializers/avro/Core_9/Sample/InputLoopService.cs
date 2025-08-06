using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Hosting;
using NServiceBus;

public class InputLoopService(IMessageSession messageSession) : BackgroundService
{
    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        #region message

        var message = new CreateOrder
        {
            OrderId = 9,
            Date = DateTime.Now,
            CustomerId = 12,
            OrderItems =
            [
                new OrderItem
                {
                    ItemId = 6,
                    Quantity = 2
                },

                new OrderItem
                {
                    ItemId = 5,
                    Quantity = 4
                }
            ]
        };

        await messageSession.SendLocal(message, cancellationToken: stoppingToken);
        Console.WriteLine("Order Sent");

        #endregion
    }
}