namespace Snippets6.Serialization
{
    using NServiceBus;

    public class JsonSerializerUsage
    {
        public void Simple()
        {

            #region JsonSerialization

            EndpointConfiguration endpointConfiguration = new EndpointConfiguration();
            endpointConfiguration.UseSerialization<JsonSerializer>();

            #endregion
        }
    }
}