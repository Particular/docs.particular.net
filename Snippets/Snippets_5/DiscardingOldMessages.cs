using System;
using NServiceBus;

public class DiscardingOldMessages
{

    #region DiscardingOldMessagesWithAnAttribute 5
    [TimeToBeReceived("00:01:00")] // Discard after one minute
    public class MyMessage { }
    #endregion

    public void Simple()
    {
        #region DiscardingOldMessagesWithFluent 5

        var configuration = new BusConfiguration();

        configuration.Conventions().DefiningTimeToBeReceivedAs(type =>
        {
            if (type == typeof (MyMessage))
            {
                return TimeSpan.FromHours(1);
            }
            return TimeSpan.MaxValue;
        });

        #endregion
    }

}