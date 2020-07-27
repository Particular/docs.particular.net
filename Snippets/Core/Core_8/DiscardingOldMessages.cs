namespace Core8
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

        DiscardingOldMessages(EndpointConfiguration endpointConfiguration)
        {
            #region DiscardingOldMessagesWithCode

            var conventions = endpointConfiguration.Conventions();
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