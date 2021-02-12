using System;
namespace Core6.Routing
{
    using NServiceBus;

    class InstanceMapping
    {
        public void InstanceMappingFileRefreshInterval(EndpointConfiguration endpointConfiguration)
        {
            #region InstanceMappingFile-RefreshInterval

            var routing = endpointConfiguration.UseTransport(new MsmqTransport());
            var instanceMappingFile = routing.InstanceMappingFile();
            var fileSettings = instanceMappingFile.FilePath(@"C:\instance-mapping.xml");
            fileSettings.RefreshInterval(TimeSpan.FromSeconds(45));

            #endregion
        }

        public void InstanceMappingFilePath(EndpointConfiguration endpointConfiguration)
        {
            #region InstanceMappingFile-FilePath

            var routing = endpointConfiguration.UseTransport(new MsmqTransport());
            var instanceMappingFile = routing.InstanceMappingFile();
            instanceMappingFile.FilePath(@"C:\instance-mapping.xml");

            #endregion
        }

        public void InstanceMappingUriPath(EndpointConfiguration endpointConfiguration)
        {
            #region InstanceMappingFile-UriPath

            var routing = endpointConfiguration.UseTransport(new MsmqTransport());
            var instanceMappingFile = routing.InstanceMappingFile();
            instanceMappingFile.Path(new Uri("http://myserver/instance-mapping.xml"));

            #endregion
        }

        public void StrictSchemaValidation(EndpointConfiguration endpointConfiguration)
        {
            #region Strict-Schema-Validation

            var routing = endpointConfiguration.UseTransport(new MsmqTransport());
            var instanceMappingFile = routing.InstanceMappingFile();
            instanceMappingFile.EnforceStrictSchemaValidation();

            #endregion
        }
    }
}
