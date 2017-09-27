using System.Security.Cryptography;
using System.Text;
using Newtonsoft.Json;
using Newtonsoft.Json.Encryption;
using NServiceBus;
using NServiceBus.Newtonsoft.Encryption;

public static class EncryptionExtensions
{
    #region ConfigureEncryption

    public static void ConfigurationEncryption(this EndpointConfiguration endpointConfiguration)
    {
        var key = Encoding.UTF8.GetBytes("gdDbqRpqdRbTs3mhdZh9qCaDaxJXl+e6");
        var serialization = endpointConfiguration.UseSerialization<NewtonsoftSerializer>();
        var encryptionFactory = new EncryptionFactory();
        serialization.Settings(
            new JsonSerializerSettings
            {
                ContractResolver = encryptionFactory.GetContractResolver()
            });

        endpointConfiguration.EnableJsonEncryption(
            encryptionFactory: encryptionFactory,
            encryptStateBuilder: () =>
                (
                algorithm: new RijndaelManaged
                {
                    Key = key
                },
                keyId: "1"
                ),
            decryptStateBuilder: (keyId, initVector) =>
                new RijndaelManaged
                {
                    Key = key,
                    IV = initVector
                });
    }

    #endregion
}