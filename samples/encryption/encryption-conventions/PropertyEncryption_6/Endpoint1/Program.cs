using System;
using System.Collections.Generic;
using NServiceBus;

Console.Title = "Endpoint1";
var endpointConfiguration = new EndpointConfiguration("Samples.Encryption.Endpoint1");
endpointConfiguration.Conventions().DefiningMessagesAs(type => type.Name.Contains("Message"));
#region enableEncryption
endpointConfiguration.ConfigurationEncryption();
#endregion
endpointConfiguration.UsePersistence<LearningPersistence>();
endpointConfiguration.UseTransport(new LearningTransport());
endpointConfiguration.UseSerialization<SystemJsonSerializer>();
var endpointInstance = await Endpoint.Start(endpointConfiguration);
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
await endpointInstance.Send("Samples.Encryption.Endpoint2", message);

Console.WriteLine("MessageWithSecretData sent. Press any key to exit");
Console.ReadKey();
await endpointInstance.Stop();