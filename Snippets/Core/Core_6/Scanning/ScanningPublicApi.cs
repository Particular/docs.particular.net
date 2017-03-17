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

            endpointConfiguration.ScanAssembliesInNestedDirectories();

            #endregion
        }

        void ScanningExcludeByName(EndpointConfiguration endpointConfiguration)
        {
            #region ScanningExcludeByName

            endpointConfiguration.ExcludeAssemblies("MyAssembly1.dll", "MyAssembly2.dll");

            #endregion
        }

        void ScanningExcludeByWildcard(EndpointConfiguration endpointConfiguration)
        {
            #region ScanningAssebliesWildcard

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
                        endpointConfiguration.ExcludeAssemblies(fileName);
                        break;
                    }
                }
            }

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

            endpointConfiguration.ExcludeAssemblies(
                "BadAssembly1.dll",
                "BadAssembly2.dll");

            #endregion
        }

    }
}