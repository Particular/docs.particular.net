namespace Snippets5.Serialization
{
    using NServiceBus;

    public class JsonSerializerUsage
    {
        public void Simple()
        {

            BusConfiguration busConfiguration = new BusConfiguration();
            #region JsonSerialization

            busConfiguration.UseSerialization<JsonSerializer>();

            #endregion
        }


    }

}