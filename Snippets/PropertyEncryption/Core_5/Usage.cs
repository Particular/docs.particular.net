namespace Core5.Encryption
{
    using System;
    using System.Collections.Generic;
    using System.Text;
    using NServiceBus;

    class Usage
    {
        Usage(BusConfiguration busConfiguration)
        {
            #region EncryptionServiceSimple

            busConfiguration.RijndaelEncryptionService();

            #endregion
        }


        void FromCode(BusConfiguration busConfiguration)
        {
#pragma warning disable 618
            #region EncryptionFromCode

            var encryptionKeyIdentifier = "2015-10";
            var encryptionKey = Convert.FromBase64String("gdDbqRpqdRbTs3mhdZh9qCaDaxJXl+e6");
            var expiredKeys = new List<byte[]>
            {
                Encoding.ASCII.GetBytes("abDbqRpQdRbTs3mhdZh9qCaDaxJXl+e6"),
                Encoding.ASCII.GetBytes("cdDbqRpQdRbTs3mhdZh9qCaDaxJXl+e6")
            };
            busConfiguration.RijndaelEncryptionService(encryptionKeyIdentifier, encryptionKey, expiredKeys);

            #endregion
#pragma warning restore 618
        }

    }
}