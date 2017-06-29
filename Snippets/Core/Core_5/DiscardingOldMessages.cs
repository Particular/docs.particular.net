namespace Core5
{
    using System;
    using NServiceBus;

    class DiscardingOldMessages
    {

        #region DiscardingOldMessagesWithAnAttribute

        // Discard after one minute
        [TimeToBeReceived("00:01:00")]
        public class MyMessage
        {
        }

        #endregion

        DiscardingOldMessages(BusConfiguration busConfiguration)
        {
            #region DiscardingOldMessagesWithCode

            var conventions = busConfiguration.Conventions();
            conventions.DefiningTimeToBeReceivedAs(
                type =>
                {
                    if (type == typeof(MyMessage))
                    {
                        return TimeSpan.FromMinutes(1);
                    }
                    return TimeSpan.MaxValue;
                });

            #endregion
        }

    }
}