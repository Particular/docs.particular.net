namespace Snippets6.Mutators
{
    using NServiceBus;

    public class MutatorRegistration
    {
        public void Simple()
        {
            #region MutatorRegistration
            BusConfiguration busConfiguration = new BusConfiguration();
            busConfiguration.RegisterComponents(c => c.ConfigureComponent<MyMutator>(DependencyLifecycle.InstancePerCall));
            #endregion
        }

        public class MyMutator
        {
        }
    }
}

