namespace Core3.Serialization
{
    using NServiceBus;

    class JsonSerializerUsage
    {
        JsonSerializerUsage(Configure configure)
        {
            #region JsonSerialization

            configure.JsonSerializer();

            #endregion
        }
    }
}