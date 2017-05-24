﻿namespace Core6.UpgradeGuides._5to6
{
    using NServiceBus;

    public class Usage
    {
        void DisableLegacyRetriesSatellite(EndpointConfiguration endpointConfiguration)
        {
#pragma warning disable 618
            #region DisableLegacyRetriesSatellite

            var recoverability = endpointConfiguration.Recoverability();
            recoverability.DisableLegacyRetriesSatellite();

            #endregion
#pragma warning restore 618
        }
    }
}