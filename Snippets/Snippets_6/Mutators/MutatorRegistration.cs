namespace Snippets6.Mutators
{
    using NServiceBus;

    public class MutatorRegistration
    {
        public void Simple()
        {
            #region MutatorRegistration
            EndpointConfiguration endpointConfiguration = new EndpointConfiguration();
            endpointConfiguration.RegisterComponents(c => c.ConfigureComponent<MyMutator>(DependencyLifecycle.InstancePerCall));
            #endregion
        }

        public class MyMutator
        {
        }
    }
}

