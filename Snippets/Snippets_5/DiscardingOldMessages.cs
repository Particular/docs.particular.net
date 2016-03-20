namespace Snippets5
{
    using System;
    using NServiceBus;

    public class DiscardingOldMessages
    {

        #region DiscardingOldMessagesWithAnAttribute
        [TimeToBeReceived("00:01:00")] // Discard after one minute
        public class MyMessage { }
        #endregion

        DiscardingOldMessages(BusConfiguration busConfiguration)
        {
            #region DiscardingOldMessagesWithCode
            busConfiguration.Conventions()
                .DefiningTimeToBeReceivedAs(type =>
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
}