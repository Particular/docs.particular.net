using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using NServiceBus;

class Program
{
    static async Task Main()
    {
        Console.Title = "Samples.Encryption.Endpoint1";
        var endpointConfiguration = new EndpointConfiguration("Samples.Encryption.Endpoint1");
        #region enableEncryption
        endpointConfiguration.ConfigurationEncryption();
        #endregion
        endpointConfiguration.UsePersistence<LearningPersistence>();
        endpointConfiguration.UseTransport<LearningTransport>();
        var endpointInstance = await Endpoint.Start(endpointConfiguration)
            .ConfigureAwait(false);
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
        await endpointInstance.Send("Samples.Encryption.Endpoint2", message)
            .ConfigureAwait(false);

        Console.WriteLine("MessageWithSecretData sent. Press any key to exit");
        Console.ReadKey();
        await endpointInstance.Stop()
            .ConfigureAwait(false);
    }
}