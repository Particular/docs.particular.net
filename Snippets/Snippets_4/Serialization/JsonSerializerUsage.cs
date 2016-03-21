namespace Snippets4.Serialization
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