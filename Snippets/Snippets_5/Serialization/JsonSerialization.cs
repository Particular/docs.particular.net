namespace Snippets5
{
    using NServiceBus;

    public class JsonSerialization
    {
        public void Simple()
        {

            #region JsonSerialization

            BusConfiguration busConfiguration = new BusConfiguration();
            busConfiguration.UseSerialization<JsonSerializer>();

            #endregion
        }


    }

}