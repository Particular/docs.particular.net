namespace Snippets4
{
    using System;
    using NServiceBus;

    public class DiscardingOldMessages
    {

        #region DiscardingOldMessagesWithAnAttribute
        [TimeToBeReceived("00:01:00")] // Discard after one minute
        public class MyMessage { }
        #endregion

        public void Simple()
        {
            #region DiscardingOldMessagesWithCode

            Configure configure = Configure.With();
            configure.DefiningTimeToBeReceivedAs(type =>
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