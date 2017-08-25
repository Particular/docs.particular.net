namespace Core6.Encryption
{
    using System;
    using System.Collections.Generic;
    using NServiceBus;
    using NServiceBus.Encryption.MessageProperty;

    class Usage
    {
        Usage(EndpointConfiguration endpointConfiguration)
        {
            #region EncryptionServiceSimple

            var defaultKey = "2015-10";
            var encryptionService = new RijndaelEncryptionService(
                encryptionKeyIdentifier: defaultKey,
                key: Convert.FromBase64String("gdDbqRpqdRbTs3mhdZh9qCaDaxJXl+e6"));

            endpointConfiguration.EnableMessagePropertyEncryption(encryptionService);

            #endregion
        }

        void FromCode(EndpointConfiguration endpointConfiguration)
        {
            #region EncryptionFromCode

            var defaultKey = "2015-10";

            var keys = new Dictionary<string, byte[]>
            {
                {"2015-10", Convert.FromBase64String("gdDbqRpqdRbTs3mhdZh9qCaDaxJXl+e6")},
                {"2015-09", Convert.FromBase64String("abDbqRpQdRbTs3mhdZh9qCaDaxJXl+e6")},
                {"2015-08", Convert.FromBase64String("cdDbqRpQdRbTs3mhdZh9qCaDaxJXl+e6")},
            };
            var encryptionService = new RijndaelEncryptionService(defaultKey, keys);

            endpointConfiguration.EnableMessagePropertyEncryption(encryptionService);

            #endregion
        }
    }
}