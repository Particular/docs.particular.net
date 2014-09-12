using NServiceBus;
using NServiceBus.Config.ConfigurationSource;

public class CustomConfigSource
{
    public CustomConfigSource()
    {

        #region RegisterCustomConfigSource-v5

        var configuration = new BusConfiguration();

        configuration.CustomConfigurationSource(new MyCustomConfigurationSource());

        #endregion RegisterCustomConfigSource-v5
    }

}

public class MyCustomConfigurationSource : IConfigurationSource
{
    public T GetConfiguration<T>() where T : class, new()
    {
        throw new System.NotImplementedException();
    }
}
