namespace Core6
{
    using System;
    using NServiceBus;

    class DiscardingOldMessages
    {

        #region DiscardingOldMessagesWithAnAttribute

        [TimeToBeReceived("00:01:00")] // Discard after one minute
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
                        return TimeSpan.FromHours(1);
                    }
                    return TimeSpan.MaxValue;
                });

            #endregion
        }

    }
}