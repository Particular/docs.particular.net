using System;
namespace Core6.Routing
{
    using NServiceBus;

    class InstanceMapping
    {
        void InstanceMappingFileConfig(EndpointConfiguration endpointConfiguration)
        {
            #region InstanceMappingFile-Config

            var transport = endpointConfiguration.UseTransport<MsmqTransport>();
            var routing = transport.Routing();
            var routingTable = routing.InstanceMappingFile();
            routingTable.FilePath(@"C:\Routes.xml");
            routing.RouteToEndpoint(typeof(AcceptOrder), "Sales");
            routing.RouteToEndpoint(typeof(SendOrder), "Shipping");

            #endregion
        }

        public void InstanceMappingFileRefreshInterval(EndpointConfiguration endpointConfiguration)
        {
            #region InstanceMappingFile-RefreshInterval


            var transport = endpointConfiguration.UseTransport<MsmqTransport>();
            var routing = transport.Routing();
            var routingTable = routing.InstanceMappingFile();
            var fileRoutingTable = routingTable.FilePath(@"C:\Routes.xml");
            fileRoutingTable.RefreshInterval(TimeSpan.FromSeconds(45));

            #endregion
        }

        public void InstanceMappingFilePath(EndpointConfiguration endpointConfiguration)
        {
            #region InstanceMappingFile-FilePath

            var transport = endpointConfiguration.UseTransport<MsmqTransport>();
            var routing = transport.Routing();
            var routingTable = routing.InstanceMappingFile();
            routingTable.FilePath(@"C:\Routes.xml");

            #endregion
        }

        class AcceptOrder
        {
        }

        class SendOrder
        {
        }
    }
}
