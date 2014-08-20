using NServiceBus;

public class SecondLevelRetriesConfig
{
    public void Disable()
    {
        #region SecondLevelRetriesDisableV3

        Configure.Instance.DisableSecondLevelRetries();

        #endregion
    }

}