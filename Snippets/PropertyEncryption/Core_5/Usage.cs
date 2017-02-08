namespace Core5.Encryption
{
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
            #region EncryptionFromCode

            var defaultKey = "2015-10";

            var ascii = Encoding.ASCII;
            var keys = new Dictionary<string, byte[]>
            {
                {"2015-10", ascii.GetBytes("gdDbqRpqdRbTs3mhdZh9qCaDaxJXl+e6")},
                {"2015-09", ascii.GetBytes("abDbqRpQdRbTs3mhdZh9qCaDaxJXl+e6")},
                {"2015-08", ascii.GetBytes("cdDbqRpQdRbTs3mhdZh9qCaDaxJXl+e6")},
            };
            busConfiguration.RijndaelEncryptionService(defaultKey, keys);

            #endregion
        }

    }
}