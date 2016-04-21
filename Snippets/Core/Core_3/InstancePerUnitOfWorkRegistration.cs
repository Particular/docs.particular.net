namespace Core3
{
    using NServiceBus;

    class InstancePerUnitOfWorkRegistration
    {
        InstancePerUnitOfWorkRegistration(Configure configuration)
        {
            #region InstancePerUnitOfWorkRegistration

            var configureComponents = configuration.Configurer;
            configureComponents.ConfigureComponent<MyService>(DependencyLifecycle.InstancePerCall);

            #endregion
        }

        public class MyService
        {
        }
    }
}

