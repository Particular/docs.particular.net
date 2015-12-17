namespace Snippets6.Scanning
{
    using System;
    using NServiceBus;

    public class ScanningPublicApi
    {
        public void ScanningDefault()
        {
            #region ScanningDefault

            BusConfiguration busConfiguration = new BusConfiguration();

            #endregion
        }

        public void ScanningNestedAssembliesEnabled()
        {
            BusConfiguration busConfiguration = new BusConfiguration();

            #region ScanningNestedAssebliesEnabled

            busConfiguration.ScanAssembliesInNestedDirectories();

            #endregion
        }

        public void ScanningExcludeByName()
        {
            BusConfiguration busConfiguration = new BusConfiguration();

            #region ScanningExcludeByName

            busConfiguration.ExcludeAssemblies("MyAssembly1.dll", "MyAssembly2.dll");

            #endregion
        }

        public void ScanningExcludeTypes()
        {
            BusConfiguration busConfiguration = new BusConfiguration();
            Type type1 = null;
            Type type2 = null;

            #region ScanningExcludeTypes

            busConfiguration.ExcludeTypes(type1, type2);

            #endregion
        }

        public void ScanningUpgrade()
        {
            BusConfiguration busConfiguration = new BusConfiguration();

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