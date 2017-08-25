namespace Core6.UpgradeGuides.Split.Conventions
{
    using System;
    using NServiceBus;
    using NServiceBus.Encryption.MessageProperty;

    class Usage
    {
        Usage(EndpointConfiguration endpointConfiguration)
        {
            #region SplitDefiningEncryptedPropertiesAs

            var encryptionService = new RijndaelEncryptionService(
                encryptionKeyIdentifier: "2015-10",
                key: Convert.FromBase64String("gdDbqRpqdRbTs3mhdZh9qCaDaxJXl+e6"));

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