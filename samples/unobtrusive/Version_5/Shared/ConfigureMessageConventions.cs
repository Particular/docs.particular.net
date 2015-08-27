using NServiceBus;

    class ConfigureMessageConventions : INeedInitialization
    {
        public void Customize(BusConfiguration configuration)
        {
            configuration.ApplyCustomConventions();
        }
    }
