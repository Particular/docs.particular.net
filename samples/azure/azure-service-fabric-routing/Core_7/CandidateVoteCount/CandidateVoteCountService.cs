using System.Collections.Generic;
using System.Fabric;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.ServiceFabric.Services.Communication.Runtime;
using Microsoft.ServiceFabric.Services.Runtime;

class CandidateVoteCountService :
    StatefulService
{
    EndpointCommunicationListener listener;

    public CandidateVoteCountService(StatefulServiceContext context)
        : base(context)
    {
    }

    protected override IEnumerable<ServiceReplicaListener> CreateServiceReplicaListeners()
    {
        listener = new EndpointCommunicationListener(Context, StateManager);
        return new List<ServiceReplicaListener>
        {
            new ServiceReplicaListener(context => listener)
        };
    }

    protected override Task RunAsync(CancellationToken cancellationToken)
    {
        return listener.Run();
    }
}