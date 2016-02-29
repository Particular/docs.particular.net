namespace Snippets6.Scanning
{
    using System;
    using NServiceBus;

    public class ScanningPublicApi
    {
        public void ScanningDefault()
        {
            #region ScanningDefault

            EndpointConfiguration endpointConfiguration = new EndpointConfiguration();

            #endregion
        }

        public void ScanningNestedAssembliesEnabled()
        {
            EndpointConfiguration endpointConfiguration = new EndpointConfiguration();

            #region ScanningNestedAssebliesEnabled

            endpointConfiguration.ScanAssembliesInNestedDirectories();

            #endregion
        }

        public void ScanningExcludeByName()
        {
            EndpointConfiguration endpointConfiguration = new EndpointConfiguration();

            #region ScanningExcludeByName

            endpointConfiguration.ExcludeAssemblies("MyAssembly1.dll", "MyAssembly2.dll");

            #endregion
        }

        public void ScanningExcludeTypes()
        {
            EndpointConfiguration endpointConfiguration = new EndpointConfiguration();
            Type type1 = null;
            Type type2 = null;

            #region ScanningExcludeTypes

            endpointConfiguration.ExcludeTypes(type1, type2);

            #endregion
        }

        public void ScanningUpgrade()
        {
            EndpointConfiguration endpointConfiguration = new EndpointConfiguration();

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