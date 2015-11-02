namespace Snippets4.Errors.SecondLevel.SecondLevelRetries
{
    using System;
    using NServiceBus;

    public class Usage
    {
        public Usage()
        {

            #region SecondLevelRetriesCustomPolicy

            Configure.Features.SecondLevelRetries(s => s.CustomRetryPolicy(MyCustomRetryPolicy));

            #endregion
        }

        #region SecondLevelRetriesCustomPolicyHandler
        TimeSpan MyCustomRetryPolicy(TransportMessage message)
        {
            // retry max 3 times
            if (GetNumberOfRetries(message) >= 3)
            {
                // sending back a TimeSpan.MinValue tells the 
                // SecondLevelRetry not to retry this message
                return TimeSpan.MinValue;
            }

            return TimeSpan.FromSeconds(5);
        }

        static int GetNumberOfRetries(TransportMessage message)
        {
            string value;
            if (message.Headers.TryGetValue(Headers.Retries, out value))
            {
                int i;
                if (int.TryParse(value, out i))
                {
                    return i;
                }
            }
            return 0;
        }
        #endregion

    }
}