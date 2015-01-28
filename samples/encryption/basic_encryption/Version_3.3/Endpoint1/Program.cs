using System;
using System.Collections.Generic;
using NServiceBus;
using NServiceBus.Installation.Environments;

class Program
{
    static void Main()
    {
        var configure = Configure.With();
        configure.DefineEndpointName("EncryptionSampleEndpoint1");
        configure.DefaultBuilder();
        configure.RijndaelEncryptionService();
        configure.MsmqTransport();
        configure.InMemorySagaPersister();
        configure.UseInMemoryTimeoutPersister();
        configure.InMemorySubscriptionStorage();
        configure.JsonSerializer();
        var bus = configure.UnicastBus()
            .CreateBus()
            .Start(() => Configure.Instance.ForInstallationOn<Windows>().Install());

        var message = new MessageWithSecretData
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
        bus.Send("EncryptionSampleEndpoint2", message);

        Console.WriteLine("MessageWithSecretData sent. Press any key to exit");
        Console.ReadLine();
    }
}