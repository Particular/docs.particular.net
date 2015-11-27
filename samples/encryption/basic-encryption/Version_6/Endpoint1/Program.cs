using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using NServiceBus;

class Program
{
    static void Main()
    {
        AsyncMain().GetAwaiter().GetResult();
    }

    static async Task AsyncMain()
    {
        BusConfiguration busConfiguration = new BusConfiguration();
        busConfiguration.EndpointName("Samples.Encryption.Endpoint1");
        busConfiguration.RijndaelEncryptionService();
        busConfiguration.UsePersistence<InMemoryPersistence>();
        busConfiguration.SendFailedMessagesTo("error");
        IEndpointInstance endpoint = await Endpoint.Start(busConfiguration);
        try
        {
            IBusContext busContext = endpoint.CreateBusContext();
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
            await busContext.Send("Samples.Encryption.Endpoint2", message);

            Console.WriteLine("MessageWithSecretData sent. Press any key to exit");
            Console.ReadKey();
        }
        finally
        {
            endpoint.Stop().GetAwaiter().GetResult();
        }
    }
}