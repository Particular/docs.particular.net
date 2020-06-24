using NServiceBus;

namespace MsmqTransport_1
{
    class TimeToBeReceived
    {
        public static void DisableNativeTimeToBeReceived(EndpointConfiguration endpointConfiguration)
        {
            #region disable-native-ttbr

            var transport = endpointConfiguration.UseTransport<MsmqTransport>();
            transport.DisableNativeTimeToBeReceivedInTransactions();

            #endregion
        }

        public static void IgnoreIncomingTTBRHeaders(EndpointConfiguration endpointConfiguration)
        {
            #region ignore-incoming-ttbr-headers

            var transport = endpointConfiguration.UseTransport<MsmqTransport>();
            transport.IgnoreIncomingTimeToBeReceivedHeaders();

            #endregion
        }
    }
}
