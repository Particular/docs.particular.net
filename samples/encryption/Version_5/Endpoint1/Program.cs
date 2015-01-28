using System;
using System.Collections.Generic;
using NServiceBus;

class Program
{
    static void Main()
    {
        var busConfiguration = new BusConfiguration();
        busConfiguration.EndpointName("EncryptionSampleEndpoint1");
        busConfiguration.RijndaelEncryptionService("gdDbqRpqdRbTs3mhdZh9qCaDaxJXl+e6");
        busConfiguration.UsePersistence<InMemoryPersistence>();
        var startableBus = Bus.Create(busConfiguration);
        using (var bus = startableBus.Start())
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
}