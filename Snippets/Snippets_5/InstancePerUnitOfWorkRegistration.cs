using NServiceBus;

public class InstancePerUnitOfWorkRegistration
{
    public void Simple()
    {
        #region InstancePerUnitOfWorkRegistration 5

        var configuration = new BusConfiguration();
        configuration.RegisterComponents(c => c.ConfigureComponent<MyService>(DependencyLifecycle.InstancePerCall));

        #endregion
    }
    public class MyService
    {
    }
}

