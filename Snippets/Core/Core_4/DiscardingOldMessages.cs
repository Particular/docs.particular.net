namespace Core4
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

        DiscardingOldMessages(Configure configure)
        {
            #region DiscardingOldMessagesWithCode

            configure.DefiningTimeToBeReceivedAs(
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