using System;
namespace Core6.Routing
{
    using NServiceBus;

    class InstanceMapping
    {
        public void InstanceMappingFileRefreshInterval(EndpointConfiguration endpointConfiguration)
        {
            #region InstanceMappingFile-RefreshInterval

            var transport = endpointConfiguration.UseTransport<MsmqTransport>();
            var routing = transport.Routing();
            var instanceMappingFile = routing.InstanceMappingFile();
            var instanceMappingFileSettings = instanceMappingFile.FilePath(@"C:\instance-mapping.xml");
            instanceMappingFileSettings.RefreshInterval(TimeSpan.FromSeconds(45));

            #endregion
        }

        public void InstanceMappingFilePath(EndpointConfiguration endpointConfiguration)
        {
            #region InstanceMappingFile-FilePath

            var transport = endpointConfiguration.UseTransport<MsmqTransport>();
            var routing = transport.Routing();
            var instanceMappingFile = routing.InstanceMappingFile();
            instanceMappingFile.FilePath(@"C:\instance-mapping.xml");

            #endregion
        }
    }
}
