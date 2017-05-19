using NServiceBus;

namespace SCTransportAdapter_0
{
    public static class MyTransportExtensions
    {
        public static TransportExtensions<API.MyTransport> UseSpecificRouting(this TransportExtensions<API.MyTransport> t)
        {
            return t;
        }
    }
}