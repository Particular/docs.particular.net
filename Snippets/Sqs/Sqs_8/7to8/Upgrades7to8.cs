using System;
using NServiceBus;

public class Upgrades7to8
{
    public void MessageVisibilityTimeout()
    {
        #region 7to8-sqs-message-visibility-timeout
        var transport = new SqsTransport
        {
            MessageVisibilityTimeout = TimeSpan.FromSeconds(10)
        };
        #endregion
    }
}