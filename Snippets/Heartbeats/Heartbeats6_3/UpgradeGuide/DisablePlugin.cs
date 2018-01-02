using NServiceBus;
using ServiceControl.Features;

class DisablePlugin
{
    public static void DisableHeartbeats(EndpointConfiguration endpointConfiguration, bool shouldSendHeartbeat)
    {
        #region NSBHeartbeat_Upgrade_Disable

        if (!shouldSendHeartbeat)
        {
            endpointConfiguration.DisableFeature<Heartbeats>();
        }

        #endregion
    }
}
