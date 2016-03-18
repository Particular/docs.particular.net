namespace Snippets6.Mutators
{
    using NServiceBus;

    public class MutatorRegistration
    {
        public void Simple(EndpointConfiguration endpointConfiguration)
        {
            #region MutatorRegistration
            endpointConfiguration.RegisterComponents(c => c.ConfigureComponent<MyMutator>(DependencyLifecycle.InstancePerCall));
            #endregion
        }

        public class MyMutator
        {
        }
    }
}

