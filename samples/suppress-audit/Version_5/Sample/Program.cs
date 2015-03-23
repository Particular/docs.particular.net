using System;
using NServiceBus;

class Program
{
    static IStartableBus bus;

    static void Main()
    {
        BusConfiguration busConfiguration = new BusConfiguration();
        busConfiguration.EndpointName("Sample.PipelineStream.Sender");
        busConfiguration.UseSerialization<JsonSerializer>();
        busConfiguration.UsePersistence<InMemoryPersistence>();
        #region configure-replace-behaviour
        busConfiguration.ReplaceAuditBehaviour();
        #endregion
        busConfiguration.EnableInstallers();
        bus = Bus.Create(busConfiguration);
        bus.Start();
        Run();
    }


    static void Run()
    {
        Console.WriteLine("Press 'A' to send a message that will be audited");
        Console.WriteLine("Press 'S' to send a message that will not be audited");
        Console.WriteLine("To exit, press Ctrl + C");

        while (true)
        {
            ConsoleKeyInfo key = Console.ReadKey();

            if (key.Key == ConsoleKey.A)
            {
                SendMessageToAudit();
                continue;
            }
            if (key.Key == ConsoleKey.S)
            {
                SendMessageToSkipAudit();
            }
        }
    }

    static void SendMessageToAudit()
    {
        #region send-message-to-audit

        MessageToAudit message = new MessageToAudit();
        bus.SendLocal(message);
        #endregion

        Console.WriteLine();
        Console.WriteLine("Message to audit sent");
    }
    static void SendMessageToSkipAudit()
    {
        #region send-message-to-skip-audit
        MessageToSkipAudit message = new MessageToSkipAudit();
            bus.SendLocal( message);
        #endregion

        Console.WriteLine();
        Console.WriteLine("Message to skip audit sent");
    }
}

