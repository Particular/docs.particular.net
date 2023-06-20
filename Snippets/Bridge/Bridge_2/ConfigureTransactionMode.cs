using NServiceBus;

class ConfigureTransactionMode
{
    void SetExplicitReceiveOnly()
    {
        #region bridge-configuration-explicit-receive-only-mode

        var bridgeConfiguration = new BridgeConfiguration();

        bridgeConfiguration.RunInReceiveOnlyTransactionMode();

        #endregion
    }
}
