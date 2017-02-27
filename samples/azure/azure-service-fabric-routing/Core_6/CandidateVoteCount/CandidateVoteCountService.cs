using System.Collections.Generic;
using System.Fabric;
using Microsoft.ServiceFabric.Services.Communication.Runtime;
using Microsoft.ServiceFabric.Services.Runtime;

class CandidateVoteCountService : StatefulService
{
    public CandidateVoteCountService(StatefulServiceContext context)
        : base(context)
    {
    }

    protected override IEnumerable<ServiceReplicaListener> CreateServiceReplicaListeners()
    {
        var listener = new EndpointCommunicationListener(Context);
        return new List<ServiceReplicaListener> { new ServiceReplicaListener(context => listener) };
    }
}
