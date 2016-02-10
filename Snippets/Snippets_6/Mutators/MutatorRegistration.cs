namespace Snippets6.Mutators
{
    using NServiceBus;

    public class MutatorRegistration
    {
        public void Simple()
        {
            #region MutatorRegistration
            EndpointConfiguration configuration = new EndpointConfiguration();
            configuration.RegisterComponents(c => c.ConfigureComponent<MyMutator>(DependencyLifecycle.InstancePerCall));
            #endregion
        }

        public class MyMutator
        {
        }
    }
}

