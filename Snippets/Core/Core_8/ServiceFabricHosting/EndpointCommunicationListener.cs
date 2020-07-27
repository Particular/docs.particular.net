#pragma warning disable 1998
namespace Core8.ServiceFabricHosting
{
    using System.Collections.Generic;
    using System.Fabric;
    using System.Threading;
    using System.Threading.Tasks;
    using Microsoft.ServiceFabric.Data;
    using Microsoft.ServiceFabric.Services.Communication.Runtime;
    using Microsoft.ServiceFabric.Services.Runtime;
    using NServiceBus;

    #region StatelessEndpointCommunicationListener

    public class StatelessEndpointCommunicationListener :
        ICommunicationListener
    {
        IEndpointInstance endpointInstance;

        public async Task<string> OpenAsync(CancellationToken cancellationToken)
        {
            var endpointName = "Sales";

            var endpointConfiguration = new EndpointConfiguration(endpointName);

            // endpoint configuration
            endpointInstance = await Endpoint.Start(endpointConfiguration)
                .ConfigureAwait(false);

            return endpointName;
        }

        public Task CloseAsync(CancellationToken cancellationToken)
        {
            return endpointInstance.Stop();
        }

        public void Abort()
        {
            CloseAsync(CancellationToken.None);
        }
    }

    #endregion

    #region StatefulEndpointCommunicationListener

    public class StatefulEndpointCommunicationListener :
        ICommunicationListener
    {
        IReliableStateManager stateManager;
        IEndpointInstance endpointInstance;
        EndpointConfiguration endpointConfiguration;

        public StatefulEndpointCommunicationListener(IReliableStateManager stateManager)
        {
            this.stateManager = stateManager;
        }

        public async Task<string> OpenAsync(CancellationToken cancellationToken)
        {
            var endpointName = "Sales";

            endpointConfiguration = new EndpointConfiguration(endpointName);

            // configure endpoint with state manager dependency

            return endpointName;
        }

        public async Task RunAsync()
        {
            endpointInstance = await Endpoint.Start(endpointConfiguration)
                .ConfigureAwait(false);
        }

        public Task CloseAsync(CancellationToken cancellationToken)
        {
            return endpointInstance.Stop();
        }

        public void Abort()
        {
            CloseAsync(CancellationToken.None);
        }
    }

    #endregion

    #region StatefulService

    class MyStatefulService :
        StatefulService
    {
        StatefulEndpointCommunicationListener listener;

        public MyStatefulService(StatefulServiceContext context)
            : base(context)
        { }

        protected override IEnumerable<ServiceReplicaListener> CreateServiceReplicaListeners()
        {
            listener = new StatefulEndpointCommunicationListener(StateManager);
            return new List<ServiceReplicaListener>
            {
                new ServiceReplicaListener(context => listener)
            };
        }

        protected override Task RunAsync(CancellationToken cancellationToken)
        {
            return listener.RunAsync();
        }
    }

    #endregion
}