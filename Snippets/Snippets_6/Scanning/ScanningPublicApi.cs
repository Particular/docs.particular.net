namespace Snippets6.Scanning
{
    using System;
    using NServiceBus;

    class ScanningPublicApi
    {
        void ScanningDefault()
        {
            #region ScanningDefault

            EndpointConfiguration endpointConfiguration = new EndpointConfiguration("EndpointName");

            #endregion
        }

        void ScanningNestedAssembliesEnabled(EndpointConfiguration endpointConfiguration)
        {
            #region ScanningNestedAssebliesEnabled

            endpointConfiguration.ScanAssembliesInNestedDirectories();

            #endregion
        }

        void ScanningExcludeByName(EndpointConfiguration endpointConfiguration)
        {
            #region ScanningExcludeByName

            endpointConfiguration.ExcludeAssemblies("MyAssembly1.dll", "MyAssembly2.dll");

            #endregion
        }

        void ScanningExcludeTypes(EndpointConfiguration endpointConfiguration, Type type1, Type type2)
        {
            #region ScanningExcludeTypes

            endpointConfiguration.ExcludeTypes(type1, type2);

            #endregion
        }

        void ScanningUpgrade(EndpointConfiguration endpointConfiguration)
        {
            #region 5to6ScanningUpgrade

            endpointConfiguration.ExcludeAssemblies("BadAssembly1.dll", "BadAssembly2.dll");

            #endregion
        }

        #region ScanningConfigurationInNSBHost

        public class EndpointConfig : IConfigureThisEndpoint
        {
            public void Customize(EndpointConfiguration endpointConfiguration)
            {
                // use 'busConfiguration' object to configure scanning
            }
        }

        #endregion
    }
}