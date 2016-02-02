﻿using System;
using System.Linq;
using System.Threading.Tasks;
using NServiceBus;
using NServiceBus.Transports.SQLServer;
using NHibernate.Cfg;
using NHibernate.Dialect;
using NServiceBus.Persistence;

class Program
{
    const string letters = "ABCDEFGHIJKLMNOPQRSTUVXYZ";
    static Random random;

    static void Main()
    {
        random = new Random();
        AsyncMain().GetAwaiter().GetResult();
    }

    static async Task AsyncMain()
    {
        BusConfiguration busConfiguration = new BusConfiguration();
        busConfiguration.SendFailedMessagesTo("error");
        busConfiguration.EnableInstallers();

        Configuration hibernateConfig = new Configuration();
        hibernateConfig.DataBaseIntegration(x =>
        {
            x.ConnectionStringName = "NServiceBus/Persistence";
            x.Dialect<MsSql2012Dialect>();
        });
        hibernateConfig.SetProperty("default_schema", "sender");

        #region SenderConfiguration

        busConfiguration.UseTransport<SqlServerTransport>()
            .DefaultSchema("sender")
            .UseSpecificSchema(e => "receiver");

        busConfiguration.UsePersistence<NHibernatePersistence>()
            .UseConfiguration(hibernateConfig);

        #endregion

        IEndpointInstance endpoint = await Endpoint.Start(busConfiguration);
        try
        {
            Console.WriteLine("Press enter to send a message");
            Console.WriteLine("Press any key to exit");

            while (true)
            {
                ConsoleKeyInfo key = Console.ReadKey();
                Console.WriteLine();

                if (key.Key != ConsoleKey.Enter)
                {
                    return;
                }

                string orderId = new string(Enumerable.Range(0, 4).Select(x => letters[random.Next(letters.Length)]).ToArray());
                await endpoint.Publish(new OrderSubmitted
                {
                    OrderId = orderId,
                    Value = random.Next(100)
                });
            }
        }
        finally
        {
            await endpoint.Stop();
        }
    }
}