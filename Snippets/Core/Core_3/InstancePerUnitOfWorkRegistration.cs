namespace Core3
{
    using NServiceBus;

    class InstancePerUnitOfWorkRegistration
    {
        InstancePerUnitOfWorkRegistration(Configure configuration)
        {
            #region InstancePerUnitOfWorkRegistration

            configuration.Configurer
                .ConfigureComponent<MyService>(DependencyLifecycle.InstancePerCall);

            #endregion
        }

        public class MyService
        {
        }
    }
}

