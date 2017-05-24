namespace Core6.UpgradeGuides._6to6._2
{
    using System.Threading.Tasks;
    using NServiceBus;
    using NServiceBus.MessageMutator;

    class Upgrade
    {
        public class MyMutator :
            IMutateOutgoingTransportMessages
        {
            public Task MutateOutgoing(MutateOutgoingTransportMessageContext context)
            {
                return Task.CompletedTask;
            }
        }

        void RegisterMutator(EndpointConfiguration endpointConfiguration)
        {
            #region 6to6-2register-mutator 6

            endpointConfiguration.RegisterComponents(
                registration: components =>
                {
                    components.ConfigureComponent<MyMutator>(DependencyLifecycle.InstancePerCall);
                });

            #endregion

            #region 6to6-2register-mutator 6.2
            endpointConfiguration.RegisterMessageMutator(new MyMutator());
            #endregion
        }
    }
}