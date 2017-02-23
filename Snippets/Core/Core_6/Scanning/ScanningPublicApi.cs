namespace Core6.Scanning
{
    using System;
    using NServiceBus;

    class ScanningPublicApi
    {
        void ScanningNestedAssembliesEnabled(EndpointConfiguration endpointConfiguration)
        {
            #region ScanningNestedAssebliesEnabled

            endpointConfiguration.AssemblyScanner(scannerConfiguration =>
            {
                scannerConfiguration.ScanAssembliesInNestedDirectories = true;
            });

            #endregion
        }

        void ScanningExcludeByName(EndpointConfiguration endpointConfiguration)
        {
            #region ScanningExcludeByName

            endpointConfiguration.AssemblyScanner(scannerConfiguration =>
            {
                scannerConfiguration.ExcludeAssemblies("MyAssembly1.dll", "MyAssembly2.dll");
            });

            #endregion
        }

        void ScanningExcludeTypes(EndpointConfiguration endpointConfiguration, Type type1, Type type2)
        {
            #region ScanningExcludeTypes

            endpointConfiguration.AssemblyScanner(scannerConfiguration =>
            {
                scannerConfiguration.ExcludeTypes(type1, type2);
            });

            #endregion
        }

        void ScanningUpgrade(EndpointConfiguration endpointConfiguration)
        {
            #region 5to6ScanningUpgrade

            endpointConfiguration.ExcludeAssemblies(
                "BadAssembly1.dll",
                "BadAssembly2.dll");

            #endregion
        }

        void ScanningApDomainAssemblies(EndpointConfiguration endpointConfiguration)
        {
            #region ScanningApDomainAssemblies

            endpointConfiguration.AssemblyScanner(scannerConfiguration =>
            {
                scannerConfiguration.ScanAppDomainAssemblies = true;
            });

            #endregion
        }

        void SwallowScanningExceptions(EndpointConfiguration endpointConfiguration)
        {
            #region SwallowScanningExceptions

            endpointConfiguration.AssemblyScanner(scannerConfiguration =>
            {
                scannerConfiguration.ThrowExceptions = false;
            });

            #endregion
        }
    }
}