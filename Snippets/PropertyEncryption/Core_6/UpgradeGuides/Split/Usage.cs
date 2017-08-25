#pragma warning disable 618
namespace Core6.UpgradeGuides.Split
{
    using System;
    using System.Collections.Generic;
    using NServiceBus;

    class Usage
    {

        void FromCode(EndpointConfiguration endpointConfiguration)
        {
            #region SplitEncryptionFromCode
            var defaultKey = "2015-10";

            var keys = new Dictionary<string, byte[]>
            {
                {"2015-10", Convert.FromBase64String("gdDbqRpqdRbTs3mhdZh9qCaDaxJXl+e6")},
                {"2015-09", Convert.FromBase64String("abDbqRpQdRbTs3mhdZh9qCaDaxJXl+e6")},
                {"2015-08", Convert.FromBase64String("cdDbqRpQdRbTs3mhdZh9qCaDaxJXl+e6")},
            };
            endpointConfiguration.RijndaelEncryptionService(defaultKey, keys);

            #endregion
        }

    }
}