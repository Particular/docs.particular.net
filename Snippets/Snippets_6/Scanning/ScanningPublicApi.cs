namespace Snippets6.Scanning
{
    using System;
    using NServiceBus;

    public class ScanningPublicApi
    {
        public ScanningPublicApi()
        {
            Type type1 = null, type2 = null;

            #region ScanningDefault

            BusConfiguration busConfiguration = new BusConfiguration();

            #endregion

            #region ScanningNestedAssebliesEnabled

            busConfiguration.ScanAssembliesInNestedDirectories();

            #endregion

            #region ScanningExcludeByName

            busConfiguration.ExcludeAssemblies("MyAssembly1.dll", "MyAssembly2.dll");

            #endregion

            #region ScanningExcludeTypes

            busConfiguration.ExcludeTypes(type1, type2);

            #endregion

            #region 5to6ScanningUpgrade

            busConfiguration.ExcludeAssemblies("BadAssembly1.dll", "BadAssembly2.dll");

            #endregion
        }

        #region ScanningConfigurationInNSBHost

        public class EndpointConfig : IConfigureThisEndpoint
        {
            public void Customize(BusConfiguration busConfiguration)
            {
                // use 'busConfiguration' object to configure scanning
            }
        }

        #endregion
    }
}