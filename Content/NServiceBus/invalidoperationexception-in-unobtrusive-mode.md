<!--
title: "InvalidOperationException in Unobtrusive Mode"
tags: ""
summary: "To avoid an InvalidOperationException, .DefiningMessagesAs (or any one of the DefiningXXXAs) should come in the Fluent configuration before.UnicastBus(). "
-->

To avoid an InvalidOperationException, .DefiningMessagesAs (or any one of the DefiningXXXAs) should come in the Fluent configuration before
.UnicastBus(). 

You can read more about the [Unobtrusive mode](unobtrusive-mode-messages.md) or see the [Unobtrusive Sample](unobtrusive-sample.md) .

See how the server side in the unobtrusive sample might look when self hosting (instead of being hosted in NServiceBus Host):


```C#
namespace Server
{
    using NServiceBus;
    using System;

    class Program
    {
        private static IBus bus;
        
        private static void BusBootstrap()
        {
            bus = Configure.With()
                .DefaultBuilder()
                .FileShareDataBus(@"..\..\..\DataBusShare\")
                .DefiningCommandsAs(t => t.Namespace != null && t.Namespace.EndsWith("Commands"))
                .DefiningEventsAs(t => t.Namespace != null && t.Namespace.EndsWith("Events"))
                .DefiningMessagesAs(t => t.Namespace == "Messages")
                .DefiningEncryptedPropertiesAs(p => p.Name.StartsWith("Encrypted"))
                .DefiningDataBusPropertiesAs(p => p.Name.EndsWith("DataBus"))
                .DefiningExpressMessagesAs(t => t.Name.EndsWith("Express"))
                .DefiningTimeToBeReceivedAs(t => t.Name.EndsWith("Expires")
                    ? TimeSpan.FromSeconds(30)
                    : TimeSpan.MaxValue)
                .XmlSerializer()
                .MsmqTransport()
                    .IsTransactional(true)
                    .PurgeOnStartup(false)
                .RijndaelEncryptionService()
                .MsmqSubscriptionStorage()
                .UnicastBus()
                    .ImpersonateSender(false)
                    .LoadMessageHandlers()
                .CreateBus()
                .Start();
        }

        static void Main()
        {
            BusBootstrap();
            CommandSender commandSender = new CommandSender();
            commandSender.Run();
            Console.ReadLine();
        }
    }
}
```




