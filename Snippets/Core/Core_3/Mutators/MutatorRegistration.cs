namespace Core3.Mutators
{
    using NServiceBus;

    class MutatorRegistration
    {
        MutatorRegistration(Configure configuration)
        {
            #region MutatorRegistration

            var configureComponents = configuration.Configurer;
            configureComponents.ConfigureComponent<MyMutator>(DependencyLifecycle.InstancePerCall);

            #endregion
        }

        public class MyMutator
        {
        }
    }
}

