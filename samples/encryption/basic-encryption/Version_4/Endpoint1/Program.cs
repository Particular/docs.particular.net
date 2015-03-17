using System;
using System.Collections.Generic;
using NServiceBus;
using NServiceBus.Installation.Environments;

class Program
{
    static void Main()
    {
        Configure.Serialization.Json();
        Configure configure = Configure.With();
        configure.DefineEndpointName("Samples.Encryption.Endpoint1");
        configure.DefaultBuilder();
        configure.RijndaelEncryptionService();
        configure.UseTransport<Msmq>();
        configure.InMemorySagaPersister();
        configure.UseInMemoryTimeoutPersister();
        configure.InMemorySubscriptionStorage();
        IBus bus = configure.UnicastBus()
            .CreateBus()
            .Start(() => Configure.Instance.ForInstallationOn<Windows>().Install());

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
                                                 ValidTo = DateTime.UtcNow.AddYears(1), Number = "312312312312312"
                                             },
                                             new CreditCardDetails
                                             {
                                                 ValidTo = DateTime.UtcNow.AddYears(2), Number = "543645546546456"
                                             }
                                         }
                       };
        bus.Send("Samples.Encryption.Endpoint2", message);

        Console.WriteLine("MessageWithSecretData sent. Press any key to exit");
        Console.ReadLine();
    }
}