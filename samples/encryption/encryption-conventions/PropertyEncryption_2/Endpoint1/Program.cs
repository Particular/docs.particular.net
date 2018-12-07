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
        endpointConfiguration.Conventions().DefiningMessagesAs(type => type.Name.Contains("Message"));
        #region enableEncryption
        endpointConfiguration.ConfigurationEncryption();
        #endregion
        endpointConfiguration.UsePersistence<LearningPersistence>();
        endpointConfiguration.UseTransport<LearningTransport>();
        var endpointInstance = await Endpoint.Start(endpointConfiguration)
            .ConfigureAwait(false);
        var message = new MessageWithSecretData
        {
            EncryptedSecret = "betcha can't guess my secret",
            SubProperty = new MySecretSubProperty
            {
                EncryptedSecret = "My sub secret"
            },
            CreditCards = new List<CreditCardDetails>
            {
                new CreditCardDetails
                {
                    ValidTo = DateTime.UtcNow.AddYears(1),
                    EncryptedNumber = "312312312312312"
                },
                new CreditCardDetails
                {
                    ValidTo = DateTime.UtcNow.AddYears(2),
                    EncryptedNumber = "543645546546456"
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