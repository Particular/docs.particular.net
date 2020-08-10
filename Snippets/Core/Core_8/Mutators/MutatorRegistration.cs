namespace Core8.Mutators
{
    using System.Threading.Tasks;
    using NServiceBus;
    using NServiceBus.MessageMutator;

    class MutatorRegistration
    {
        MutatorRegistration(EndpointConfiguration endpointConfiguration)
        {
            #region MutatorRegistration

            endpointConfiguration.RegisterMessageMutator(new MyIncomingMessageMutator());
            endpointConfiguration.RegisterMessageMutator(new MyOutgoingTransportMessageMutator());

            #endregion
        }

        public class MyIncomingMessageMutator : IMutateIncomingMessages
        {
            public Task MutateIncoming(MutateIncomingMessageContext context)
            {
                throw new System.NotImplementedException();
            }
        }

        public class MyOutgoingTransportMessageMutator : IMutateOutgoingTransportMessages
        {
            public Task MutateOutgoing(MutateOutgoingTransportMessageContext context)
            {
                throw new System.NotImplementedException();
            }
        }
    }
}

