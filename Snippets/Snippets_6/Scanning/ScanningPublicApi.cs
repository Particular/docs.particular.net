namespace Snippets6.Scanning
{
    using System;
    using NServiceBus;

    public class ScanningPublicApi
    {
        public void ScanningDefault()
        {
            #region ScanningDefault

            EndpointConfiguration configuration = new EndpointConfiguration();

            #endregion
        }

        public void ScanningNestedAssembliesEnabled()
        {
            EndpointConfiguration configuration = new EndpointConfiguration();

            #region ScanningNestedAssebliesEnabled

            configuration.ScanAssembliesInNestedDirectories();

            #endregion
        }

        public void ScanningExcludeByName()
        {
            EndpointConfiguration configuration = new EndpointConfiguration();

            #region ScanningExcludeByName

            configuration.ExcludeAssemblies("MyAssembly1.dll", "MyAssembly2.dll");

            #endregion
        }

        public void ScanningExcludeTypes()
        {
            EndpointConfiguration configuration = new EndpointConfiguration();
            Type type1 = null;
            Type type2 = null;

            #region ScanningExcludeTypes

            configuration.ExcludeTypes(type1, type2);

            #endregion
        }

        public void ScanningUpgrade()
        {
            EndpointConfiguration configuration = new EndpointConfiguration();

            #region 5to6ScanningUpgrade

            configuration.ExcludeAssemblies("BadAssembly1.dll", "BadAssembly2.dll");

            #endregion
        }

        #region ScanningConfigurationInNSBHost

        public class EndpointConfig : IConfigureThisEndpoint
        {
            public void Customize(EndpointConfiguration configuration)
            {
                // use 'busConfiguration' object to configure scanning
            }
        }

        #endregion
    }
}