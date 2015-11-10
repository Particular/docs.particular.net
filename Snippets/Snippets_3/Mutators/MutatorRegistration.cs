namespace Snippets3.Mutators
{
    using NServiceBus;

    public class MutatorRegistration
    {
        public void Simple()
        {
            #region MutatorRegistration

            Configure configuration = Configure.With();
            configuration.Configurer
                .ConfigureComponent<MyMutator>(DependencyLifecycle.InstancePerCall);

            #endregion
        }

        public class MyMutator
        {
        }
    }
}

