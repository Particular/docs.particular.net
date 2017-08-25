namespace Core5.Encryption
{
    using System;
    using System.Collections.Generic;
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
            #region EncryptionFromCode

            var defaultKey = "2015-10";

            var keys = new Dictionary<string, byte[]>
            {
                {"2015-10", Convert.FromBase64String("gdDbqRpqdRbTs3mhdZh9qCaDaxJXl+e6")},
                {"2015-09", Convert.FromBase64String("abDbqRpQdRbTs3mhdZh9qCaDaxJXl+e6")},
                {"2015-08", Convert.FromBase64String("cdDbqRpQdRbTs3mhdZh9qCaDaxJXl+e6")},
            };
            busConfiguration.RijndaelEncryptionService(defaultKey, keys);

            #endregion
        }

    }
}