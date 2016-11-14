namespace Core5.Serialization
{
    using NServiceBus;

    class JsonSerializerUsage
    {
        JsonSerializerUsage(BusConfiguration busConfiguration)
        {
            #region JsonSerialization

            busConfiguration.UseSerialization<JsonSerializer>();

            #endregion
        }


    }

}