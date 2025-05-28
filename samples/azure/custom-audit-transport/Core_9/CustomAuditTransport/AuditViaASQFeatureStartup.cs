using System.Threading;
using System.Threading.Tasks;
using NServiceBus;
using NServiceBus.Features;

namespace CustomAuditTransport
{
    class AuditViaASQFeatureStartup(EndpointConfiguration endpointConfiguration) :
        FeatureStartupTask
    {
        public static IEndpointInstance auditEndpoint;

        protected override async Task OnStart(IMessageSession session, CancellationToken cancellationToken)
        {            
            auditEndpoint = await Endpoint.Start(endpointConfiguration, cancellationToken);
        }

        protected override async Task OnStop(IMessageSession session, CancellationToken cancellationToken)
        {
            if (auditEndpoint != null)
            {
                await auditEndpoint.Stop(cancellationToken);
            }
        }
    }
}
