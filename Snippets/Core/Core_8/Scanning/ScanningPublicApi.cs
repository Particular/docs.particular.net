#pragma warning disable 618
namespace Core8.Scanning
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Text.RegularExpressions;
    using NServiceBus;

    class ScanningPublicApi
    {
        void ScanningNestedAssembliesEnabled(EndpointConfiguration endpointConfiguration)
        {
            #region ScanningNestedAssebliesEnabled

            var scanner = endpointConfiguration.AssemblyScanner();
            scanner.ScanAssembliesInNestedDirectories = true;

            #endregion
        }

        void ScanningExcludeByName(EndpointConfiguration endpointConfiguration)
        {
            #region ScanningExcludeByName

            var scanner = endpointConfiguration.AssemblyScanner();
            scanner.ExcludeAssemblies("MyAssembly1.dll", "MyAssembly2.dll");

            #endregion
        }

        void ScanningExcludeByWildcard(EndpointConfiguration endpointConfiguration)
        {
            #region ScanningAssembliesWildcard

            var scanner = endpointConfiguration.AssemblyScanner();

            var excludeRegexs = new List<string>
            {
                @"App_Web_.*\.dll",
                @".*\.resources\.dll"
            };

            var baseDirectory = AppDomain.CurrentDomain.BaseDirectory;
            foreach (var fileName in Directory.EnumerateFiles(baseDirectory, "*.dll")
                .Select(Path.GetFileName))
            {
                foreach (var pattern in excludeRegexs)
                {
                    if (Regex.IsMatch(fileName, pattern, RegexOptions.IgnoreCase))
                    {
                        scanner.ExcludeAssemblies(fileName);
                        break;
                    }
                }
            }

            #endregion
        }

        void ScanningExcludeTypes(EndpointConfiguration endpointConfiguration, Type type1, Type type2)
        {
            #region ScanningExcludeTypes

            var scanner = endpointConfiguration.AssemblyScanner();
            scanner.ExcludeTypes(type1, type2);

            #endregion
        }

        void ScanningApDomainAssemblies(EndpointConfiguration endpointConfiguration)
        {
            #region ScanningApDomainAssemblies

            var scanner = endpointConfiguration.AssemblyScanner();
            scanner.ScanAppDomainAssemblies = false;

            #endregion
        }

        void SwallowScanningExceptions(EndpointConfiguration endpointConfiguration)
        {
            #region SwallowScanningExceptions

            var scanner = endpointConfiguration.AssemblyScanner();
            scanner.ThrowExceptions = false;

            #endregion
        }

        void AdditionalAssemblyScanningPath(EndpointConfiguration endpointConfiguration)
        {
            var additionalPathToScanAssemblies = "";

            #region AdditionalAssemblyScanningPath

            var scanner = endpointConfiguration.AssemblyScanner();
            scanner.AdditionalAssemblyScanningPath = additionalPathToScanAssemblies;

            #endregion
        }
    }
}
