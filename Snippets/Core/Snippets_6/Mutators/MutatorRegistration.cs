namespace Snippets6.Mutators
{
    using NServiceBus;

    class MutatorRegistration
    {
        MutatorRegistration(EndpointConfiguration endpointConfiguration)
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

