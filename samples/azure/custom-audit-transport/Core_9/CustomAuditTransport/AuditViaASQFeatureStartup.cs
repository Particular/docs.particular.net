using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using NServiceBus.Features;
using NServiceBus;

namespace CustomAuditTransport
{
    class AuditViaASQFeatureStartup :
    FeatureStartupTask,
    IDisposable
    {
        public static IEndpointInstance auditEndpoint;
        EndpointConfiguration endpointConfiguration;

        public AuditViaASQFeatureStartup(EndpointConfiguration endpointConfiguration)
        {
            this.endpointConfiguration = endpointConfiguration;
        }

        protected override async Task OnStart(IMessageSession session, CancellationToken cancellationToken)
        {            
            auditEndpoint = await Endpoint.Start(endpointConfiguration);
        }

        protected override async Task OnStop(IMessageSession session, CancellationToken cnCancellationToken)
        {
            if (auditEndpoint != null)
            {
                await auditEndpoint.Stop();
            }
        }

        public void Dispose()
        {
            auditEndpoint = null;
        }
    }
}
