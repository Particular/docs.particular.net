﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Hosting;
using NServiceBus;
using NServiceBus.Routing;

namespace Client
{
    public class InputLoopService(IMessageSession messageSession) : BackgroundService
    {
        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            Console.WriteLine("Press '1' to send PlaceOrder - defer message handling");
            Console.WriteLine("Press '2' to send PlaceDelayedOrder - defer message delivery");
            Console.WriteLine("Press any other key to exit");

            while (true)
            {
                var key = Console.ReadKey();
                Console.WriteLine();
                var id = Guid.NewGuid();

                switch (key.Key)
                {
                    case ConsoleKey.D1:
                    case ConsoleKey.NumPad1:
                        #region SendOrder
                        var placeOrder = new PlaceOrder
                        {
                            Product = "New shoes",
                            Id = id
                        };
                        await messageSession.Send("Samples.DelayedDelivery.Server", placeOrder);
                        Console.WriteLine($"[Defer Message Handling] Sent a PlaceOrder message with id: {id.ToString("N")}");
                        #endregion
                        continue;
                    case ConsoleKey.D2:
                    case ConsoleKey.NumPad2:
                        #region DeferOrder
                        var placeDelayedOrder = new PlaceDelayedOrder
                        {
                            Product = "New shoes",
                            Id = id
                        };
                        var options = new SendOptions();

                        options.SetDestination("Samples.DelayedDelivery.Server");
                        options.DelayDeliveryWith(TimeSpan.FromSeconds(5));
                        await messageSession.Send(placeDelayedOrder, options);
                        Console.WriteLine($"[Defer Message Delivery] Deferred a PlaceDelayedOrder message with id: {id.ToString("N")}");
                        #endregion
                        continue;
                    default:
                        return;
                }
            }

        }
    }
}
