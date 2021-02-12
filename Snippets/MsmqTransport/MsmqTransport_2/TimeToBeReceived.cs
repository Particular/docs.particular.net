using NServiceBus;

namespace MsmqTransport_1
{
    class TimeToBeReceived
    {
        public static void DisableNativeTimeToBeReceived(EndpointConfiguration endpointConfiguration)
        {
            #region disable-native-ttbr

            var transport = new MsmqTransport
            {
                UseNonNativeTimeToBeReceivedInTransactions = true
            };
            endpointConfiguration.UseTransport(transport);

            #endregion
        }

        public static void IgnoreIncomingTTBRHeaders(EndpointConfiguration endpointConfiguration)
        {
            #region ignore-incoming-ttbr-headers

            var transport = new MsmqTransport
            {
                IgnoreIncomingTimeToBeReceivedHeaders = true
            };
            endpointConfiguration.UseTransport(transport);

            #endregion
        }
    }
}
