namespace Core3
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