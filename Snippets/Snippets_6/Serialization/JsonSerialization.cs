namespace Snippets6.Serialization
{
    using NServiceBus;

    public class JsonSerialization
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