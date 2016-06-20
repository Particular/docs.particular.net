namespace Core6.UpgradeGuides._5to6
{
    using NServiceBus;

    public class DisableRetries
    {
        DisableRetries(EndpointConfiguration endpointConfiguration)
        {
            #region 5to6-DisableRetries

            endpointConfiguration.DisableFirstLevelRetries();
            endpointConfiguration.SecondLevelRetries().Disable();

            #endregion
        }
    }
}