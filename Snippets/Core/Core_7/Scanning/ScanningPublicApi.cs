﻿#pragma warning disable 618
namespace Core6.Scanning
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

            var assemblyScanner = endpointConfiguration.AssemblyScanner();
            assemblyScanner.ScanAssembliesInNestedDirectories = true;

            #endregion
        }

        void ScanningExcludeByName(EndpointConfiguration endpointConfiguration)
        {
            #region ScanningExcludeByName

            var assemblyScanner = endpointConfiguration.AssemblyScanner();
            assemblyScanner.ExcludeAssemblies("MyAssembly1.dll", "MyAssembly2.dll");

            #endregion
        }

        void ScanningExcludeByWildcard(EndpointConfiguration endpointConfiguration)
        {
            #region ScanningAssembliesWildcard

            var assemblyScanner = endpointConfiguration.AssemblyScanner();

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
                        assemblyScanner.ExcludeAssemblies(fileName);
                        break;
                    }
                }
            }

            #endregion
        }

        void ScanningExcludeTypes(EndpointConfiguration endpointConfiguration, Type type1, Type type2)
        {
            #region ScanningExcludeTypes

            var assemblyScanner = endpointConfiguration.AssemblyScanner();
            assemblyScanner.ExcludeTypes(type1, type2);

            #endregion
        }

        void ScanningUpgrade(EndpointConfiguration endpointConfiguration)
        {
            #region 5to6ScanningUpgrade

            endpointConfiguration.AssemblyScanner().ExcludeAssemblies(
                "BadAssembly1.dll",
                "BadAssembly2.dll");

            #endregion
        }

        void ScanningApDomainAssemblies(EndpointConfiguration endpointConfiguration)
        {
            #region ScanningApDomainAssemblies

            var assemblyScanner = endpointConfiguration.AssemblyScanner();
            assemblyScanner.ScanAppDomainAssemblies = true;

            #endregion
        }

        void SwallowScanningExceptions(EndpointConfiguration endpointConfiguration)
        {
            #region SwallowScanningExceptions

            var assemblyScanner = endpointConfiguration.AssemblyScanner();
            assemblyScanner.ThrowExceptions = false;

            #endregion
        }
    }
}
