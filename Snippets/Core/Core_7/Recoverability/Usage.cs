namespace Core7.Recoverability
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