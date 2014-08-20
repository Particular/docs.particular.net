using NServiceBus;
using NServiceBus.Features;

public class SecondLevelRetriesConfig
{
    public void Disable()
    {
        #region SecondLevelRetriesDisableV4

        Configure.Features.Disable<SecondLevelRetries>();

        #endregion
    }

}