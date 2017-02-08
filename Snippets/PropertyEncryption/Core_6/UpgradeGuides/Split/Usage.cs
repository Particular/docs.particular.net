namespace Core6.UpgradeGuides.Split
{
    using System.Collections.Generic;
    using System.Text;
    using NServiceBus;

    class Usage
    {

        void FromCode(EndpointConfiguration endpointConfiguration)
        {
            #region SplitEncryptionFromCode
            var defaultKey = "2015-10";

            var ascii = Encoding.ASCII;
            var keys = new Dictionary<string, byte[]>
            {
                {"2015-10", ascii.GetBytes("gdDbqRpqdRbTs3mhdZh9qCaDaxJXl+e6")},
                {"2015-09", ascii.GetBytes("abDbqRpQdRbTs3mhdZh9qCaDaxJXl+e6")},
                {"2015-08", ascii.GetBytes("cdDbqRpQdRbTs3mhdZh9qCaDaxJXl+e6")},
            };
            endpointConfiguration.RijndaelEncryptionService(defaultKey, keys);

            #endregion
        }

    }
}