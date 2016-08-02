namespace Core5.UpgradeGuides._5to6
{
    using System;
    using NServiceBus;

    static class CustomRetryPolicy
    {
        static CustomRetryPolicy()
        {
            BusConfiguration busConfiguration = null;

            #region 5to6-DelayedRetriesCustomPolicy

            var retriesSettings = busConfiguration.SecondLevelRetries();
            retriesSettings.CustomRetryPolicy(MyCustomRetryPolicy);

            #endregion
        }

        #region 5to6-DelayedRetriesCustomPolicyHandler

        static TimeSpan MyCustomRetryPolicy(TransportMessage transportMessage)
        {
            var numberOfRetries = transportMessage.NumberOfRetries();
            var exceptionType = transportMessage.ExceptionType();

            // perform some logic and decide when to retry
            return TimeSpan.FromSeconds(5);
        }


        static int NumberOfRetries(this TransportMessage transportMessage)
        {
            string value;
            var headers = transportMessage.Headers;
            if (headers.TryGetValue(Headers.Retries, out value))
            {
                return int.Parse(value);
            }
            return 0;
        }

        static string ExceptionType(this TransportMessage transportMessage)
        {
            var headers = transportMessage.Headers;
            return headers["NServiceBus.ExceptionInfo.ExceptionType"];
        }

        #endregion
    }
}

