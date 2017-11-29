using NServiceBus;

class Upgrade7to8
{
    void Serializer(EndpointConfiguration endpointConfiguration)
    {
        #region 7to8_asb-backwardscompatible-serializer

        endpointConfiguration.UseSerialization<NewtonsoftSerializer>();

        #endregion
    }
}
