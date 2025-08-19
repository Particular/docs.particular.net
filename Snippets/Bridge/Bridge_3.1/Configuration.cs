using NServiceBus;

public class Configuration
{
    public void TranslateReplyToAddressForFailedMessages()
    {
        var bridgeConfiguration = new BridgeConfiguration();

        #region translate-reply-to-address-for-failed-messages

        bridgeConfiguration.TranslateReplyToAddressForFailedMessages();

        #endregion
    }
}
