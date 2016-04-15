namespace Core3.Mutators
{
    using NServiceBus;

    class MutatorRegistration
    {
        MutatorRegistration(Configure configuration)
        {
            #region MutatorRegistration

            configuration.Configurer
                .ConfigureComponent<MyMutator>(DependencyLifecycle.InstancePerCall);

            #endregion
        }

        public class MyMutator
        {
        }
    }
}

