using NServiceBus;

public class Configuration
{
    public void DoNotEnforceBestPractices()
    {
        var bridgeConfiguration = new BridgeConfiguration();

        #region do-not-enforce-best-practices

        bridgeConfiguration.DoNotEnforceBestPractices();

        #endregion
    }
}