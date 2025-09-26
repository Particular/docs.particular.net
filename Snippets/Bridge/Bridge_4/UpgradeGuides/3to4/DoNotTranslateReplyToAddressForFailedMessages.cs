using NServiceBus;

class DoNotTranslateReplyToAddressForFailedMessages
{
    public void Usage()
    {
        var bridgeConfiguration = new BridgeConfiguration();

        #region bridge-3to4-do-not-translate-reply-to-address

        bridgeConfiguration.DoNotTranslateReplyToAddressForFailedMessages();

        #endregion
    }
}