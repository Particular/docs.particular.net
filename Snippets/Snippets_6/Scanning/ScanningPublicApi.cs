namespace Snippets_6.Scanning
{
    using System;
    using System.Linq;
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

            var allTypes = new Type[] { };
            var typesToScan = new Type[] { };

            #region ScanningListOfTypes

            var typesToExclude = allTypes.Except(typesToScan).ToArray();

            busConfiguration.ExcludeTypes(typesToExclude);

            #endregion

            var allAssemblies = new string[] { };
            var assembliesToScan = new string[] { };

            #region ScanningListOfAssemblies

            var assembliesToExclude = allAssemblies.Except(assembliesToScan).ToArray();
            busConfiguration.ExcludeAssemblies(assembliesToExclude);

            #endregion
        }
    }
}