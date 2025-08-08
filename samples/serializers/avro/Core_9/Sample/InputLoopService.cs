using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Hosting;
using NServiceBus;

public class InputLoopService(IMessageSession messageSession) : BackgroundService
{
    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            Console.WriteLine("Press any key, to send a message");
            Console.ReadKey();

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
            Console.WriteLine("Message Sent");

            #endregion
        }

    }
}