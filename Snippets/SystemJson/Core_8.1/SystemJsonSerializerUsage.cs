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


            #region SystemJsonContentType

            endpointConfiguration.UseSerialization<SystemJsonSerializer>()
                .ContentType("application/json; systemjson");

            #endregion
        }
    }
}