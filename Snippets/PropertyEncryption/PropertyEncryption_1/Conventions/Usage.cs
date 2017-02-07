namespace Core6.Encryption.Conventions
{
    using System.Text;
    using NServiceBus;
    using NServiceBus.Encryption.MessageProperty;

    class Usage
    {
        Usage(EndpointConfiguration endpointConfiguration)
        {
            #region DefiningEncryptedPropertiesAs

            var ascii = Encoding.ASCII;
            var encryptionService = new RijndaelEncryptionService(
                encryptionKeyIdentifier: "2015-10",
                key: ascii.GetBytes("gdDbqRpqdRbTs3mhdZh9qCaDaxJXl+e6"));

            endpointConfiguration.EnableMessagePropertyEncryption(
                encryptionService: encryptionService,
                encryptedPropertyConvention: propertyInfo =>
                {
                    return propertyInfo.Name.EndsWith("EncryptedProperty");
                }
            );

            #endregion
        }
    }
}