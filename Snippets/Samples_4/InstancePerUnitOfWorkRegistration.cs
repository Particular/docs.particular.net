using NServiceBus;

public class InstancePerUnitOfWorkRegistration
{
    public void Simple()
    {
        #region InstancePerUnitOfWorkRegistrationV4

        var configuration = Configure.With();
        configuration.Configurer.ConfigureComponent<MyService>(DependencyLifecycle.InstancePerCall);

        #endregion
    }

    public class MyService
    {
    }
}

