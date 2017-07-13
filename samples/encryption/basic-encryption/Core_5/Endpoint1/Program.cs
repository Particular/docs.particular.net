using System;
using System.Collections.Generic;
using NServiceBus;

class Program
{
    static void Main()
    {
        Console.Title = "Samples.Encryption.Endpoint1";
        var busConfiguration = new BusConfiguration();
        busConfiguration.EndpointName("Samples.Encryption.Endpoint1");
        #region enableEncryption
        busConfiguration.ConfigurationEncryption();
        #endregion
        busConfiguration.UsePersistence<InMemoryPersistence>();
        using (var bus = Bus.Create(busConfiguration).Start())
        {
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