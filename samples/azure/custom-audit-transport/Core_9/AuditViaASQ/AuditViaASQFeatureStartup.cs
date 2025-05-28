using System;
using System.Threading;
using System.Threading.Tasks;
using NServiceBus;
using NServiceBus.Features;
using NServiceBus.Transport;

namespace CustomAuditTransport
{
    class AuditViaASQFeatureStartup() :
        FeatureStartupTask
    {
        public static IMessageDispatcher? AsqDispatcher;

        private TransportInfrastructure? transportInfrastructure;

        protected override async Task OnStart(IMessageSession session, CancellationToken cancellationToken)
        {
            var hostName = $"{nameof(AuditViaASQFeature)} host";

            var hostSettings = new HostSettings(
                hostName,
                hostName,
                new StartupDiagnosticEntries(),
                OnCriticalError,
                true,
                null);

            var transport = new AzureStorageQueueTransport("UseDevelopmentStorage=true");
            transport.DelayedDelivery.DelayedDeliveryPoisonQueue = "audit-via-asq-delayed-poison";

            transport.MessageWrapperSerializationDefinition = new SystemJsonSerializer();
            transportInfrastructure = await transport.Initialize(hostSettings, [], [], cancellationToken);
            AsqDispatcher = transportInfrastructure.Dispatcher;
        }

        protected override async Task OnStop(IMessageSession session, CancellationToken cancellationToken)
        {
            if (transportInfrastructure != null)
            {
                await transportInfrastructure.Shutdown(cancellationToken);
            }
        }

        public static void OnCriticalError(string errorMessage, Exception exception, CancellationToken cancellationToken = default)
        {
        }
    }
}
