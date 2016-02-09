using System;
using System.Collections.Generic;
using NServiceBus;
using NServiceBus.Installation.Environments;

class Program
{
    static void Main()
    {
        Configure configure = Configure.With();
        configure.Log4Net();
        configure.DefineEndpointName("Samples.Encryption.Endpoint1");
        configure.DefaultBuilder();
        #region enableEncryption
        configure.RijndaelEncryptionService();
        #endregion
        configure.MsmqTransport();
        configure.InMemorySagaPersister();
        configure.RunTimeoutManagerWithInMemoryPersistence();
        configure.InMemorySubscriptionStorage();
        configure.JsonSerializer();
        using (IStartableBus startableBus = configure.UnicastBus().CreateBus())
        {
            IBus bus = startableBus.Start(() => configure.ForInstallationOn<Windows>().Install());
            MessageWithSecretData message = new MessageWithSecretData
            {
                Secret = "betcha can't guess my secret",
                SubProperty = new MySecretSubProperty
                {
                    Secret = "My sub secret"
                },
                CreditCards = new List<CreditCardDetails>
                {
                    new CreditCardDetails
                    {
                        ValidTo = DateTime.UtcNow.AddYears(1),
                        Number = "312312312312312"
                    },
                    new CreditCardDetails
                    {
                        ValidTo = DateTime.UtcNow.AddYears(2),
                        Number = "543645546546456"
                    }
                }
            };
            bus.Send("Samples.Encryption.Endpoint2", message);

            Console.WriteLine("MessageWithSecretData sent. Press any key to exit");
            Console.ReadKey();
        }
    }
}