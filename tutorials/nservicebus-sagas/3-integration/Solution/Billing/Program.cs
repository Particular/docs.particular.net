﻿using System;
using System.Threading.Tasks;
using NServiceBus;

namespace Billing
{
    class Program
    {
        static async Task Main()
        {
            Console.Title = "Billing";

            var endpointConfiguration = new EndpointConfiguration("Billing");

            var transport = endpointConfiguration.UseTransport<LearningTransport>();
            var persistence = endpointConfiguration.UsePersistence<LearningPersistence>();

            endpointConfiguration.RegisterComponents(
                c =>
                {
                    c.ConfigureComponent<OrderCalculator>(DependencyLifecycle.SingleInstance);
                }
                );

            var endpointInstance = await Endpoint.Start(endpointConfiguration);

            Console.WriteLine("Press Enter to exit.");
            Console.ReadLine();

            await endpointInstance.Stop();
        }
    }
}