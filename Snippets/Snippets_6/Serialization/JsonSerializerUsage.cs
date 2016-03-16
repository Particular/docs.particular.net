namespace Snippets6.Serialization
{
    using NServiceBus;

    public class JsonSerializerUsage
    {
        public void Simple()
        {

            EndpointConfiguration endpointConfiguration = new EndpointConfiguration();
            #region JsonSerialization

            endpointConfiguration.UseSerialization<JsonSerializer>();

            #endregion
        }
    }
}