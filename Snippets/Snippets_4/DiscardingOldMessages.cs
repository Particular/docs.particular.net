using System;
using NServiceBus;

public class DiscardingOldMessages
{

    #region DiscardingOldMessagesWithAnAttributeV4
    [TimeToBeReceived("00:01:00")] // Discard after one minute
    public class MyMessage { }
    #endregion

    public void Simple()
    {
        #region DiscardingOldMessagesWithFluentV4
        var configure = Configure.With()
            .DefiningTimeToBeReceivedAs(type => {
                                                    if (type == typeof (MyMessage))
                                                    {
                                                        return TimeSpan.FromHours(1);
                                                    }
                                                    return TimeSpan.MaxValue;
            });

        #endregion
    }

}