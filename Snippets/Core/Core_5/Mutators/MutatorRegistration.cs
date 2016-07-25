namespace Core5.Mutators
{
    using NServiceBus;

    class MutatorRegistration
    {
        MutatorRegistration(BusConfiguration busConfiguration)
        {
            #region MutatorRegistration
            busConfiguration.RegisterComponents(
                registration: components =>
                {
                    components.ConfigureComponent<MyMutator>(DependencyLifecycle.InstancePerCall);
                });
            #endregion
        }

        public class MyMutator
        {
        }
    }
}

