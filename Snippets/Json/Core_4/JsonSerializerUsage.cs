namespace Core4
{
    using NServiceBus;

    class JsonSerializerUsage
    {
        JsonSerializerUsage()
        {
            #region JsonSerialization

            Configure.Serialization.Json();

            #endregion

        }

    }
}