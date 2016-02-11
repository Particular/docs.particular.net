namespace Snippets6.Serialization
{
    using NServiceBus;

    public class JsonSerializerUsage
    {
        public void Simple()
        {

            #region JsonSerialization

            EndpointConfiguration configuration = new EndpointConfiguration();
            configuration.UseSerialization<JsonSerializer>();

            #endregion
        }
    }
}