namespace Core8
{
    using NServiceBus;

    class SystemJsonSerializerUsage
    {
        SystemJsonSerializerUsage(EndpointConfiguration endpointConfiguration)
        {
            #region SystemJsonSerialization

            endpointConfiguration.UseSerialization<SystemJsonSerializer>();

            #endregion
        }
    }
}