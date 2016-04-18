using System;
using NServiceBus;
using NServiceBus.Installation.Environments;

class Program
{
    static void Main()
    {
        Console.Title = "Samples.MessageBodyEncryption.Endpoint1";
        Configure configure = Configure.With();
        configure.Log4Net();
        configure.DefineEndpointName("Samples.MessageBodyEncryption.Endpoint1");
        configure.DefaultBuilder();
        configure.RijndaelEncryptionService();
        configure.MsmqTransport();
        configure.InMemorySagaPersister();
        configure.RunTimeoutManagerWithInMemoryPersistence();
        configure.InMemorySubscriptionStorage();
        configure.JsonSerializer();
        #region RegisterMessageEncryptor
        configure.RegisterMessageEncryptor();
        #endregion
        using (IStartableBus startableBus = configure.UnicastBus().CreateBus())
        {
            IBus bus = startableBus.Start(() => configure.ForInstallationOn<Windows>().Install());

            CompleteOrder completeOrder = new CompleteOrder
            {
                CreditCard = "123-456-789"
            };
            bus.Send("Samples.MessageBodyEncryption.Endpoint2", completeOrder);

            Console.WriteLine("Message sent.");
            Console.WriteLine("Press any key to exit");
            Console.ReadKey();
        }
    }
}