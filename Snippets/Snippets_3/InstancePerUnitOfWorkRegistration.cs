using NServiceBus;

public class InstancePerUnitOfWorkRegistration
{
    public void Simple()
    {
        #region InstancePerUnitOfWorkRegistration

        Configure configuration = Configure.With();
        configuration.Configurer.ConfigureComponent<MyService>(DependencyLifecycle.InstancePerCall);

        #endregion
    }

    public class MyService
    {
    }
}

