namespace Snippets5.Scanning
{
    using System;
    using System.Collections.Generic;
    using System.Reflection;
    using NServiceBus;

    class ScanningPublicApi
    {
        void ScanningDefault()
        {
            #region ScanningDefault

            BusConfiguration busConfiguration = new BusConfiguration();

            #endregion
        }

        void ScanningExcludeByName(BusConfiguration busConfiguration)
        {
            #region ScanningExcludeByName

            IExcludesBuilder excludesBuilder = AllAssemblies
                .Except("MyAssembly1.dll")
                .And("MyAssembly2.dll");
            busConfiguration.AssembliesToScan(excludesBuilder);

            #endregion
        }

        void ScanningListOfTypes(BusConfiguration busConfiguration, IEnumerable<Type> myTypes)
        {
            #region ScanningListOfTypes

            busConfiguration.TypesToScan(myTypes);

            #endregion
        }

        void ScanningListOfAssemblies(BusConfiguration busConfiguration, IEnumerable<Assembly> myListOfAssemblies, Assembly assembly2, Assembly assembly1)
        {
            #region ScanningListOfAssemblies

            busConfiguration.AssembliesToScan(myListOfAssemblies);
            // or
            busConfiguration.AssembliesToScan(assembly1, assembly2);

            #endregion
        }

        void ScanningIncludeByPattern(BusConfiguration busConfiguration)
        {
            #region ScanningIncludeByPattern

            IIncludesBuilder includesBuilder = AllAssemblies
                .Matching("NServiceBus")
                .And("MyCompany.")
                .And("SomethingElse");
            busConfiguration.AssembliesToScan(includesBuilder);

            #endregion
        }

        void ScanningCustomDirectory(BusConfiguration busConfiguration)
        {
            #region ScanningCustomDirectory

            busConfiguration.ScanAssembliesInDirectory(@"c:\my-custom-dir");

            #endregion
        }

        void ScanningMixingIncludeAndExclude(BusConfiguration busConfiguration)
        {
            #region ScanningMixingIncludeAndExclude

            IExcludesBuilder excludesBuilder = AllAssemblies
                .Matching("NServiceBus")
                .And("MyCompany.")
                .Except("BadAssembly.dll");
            busConfiguration.AssembliesToScan(excludesBuilder);

            #endregion

        }

        void ScanningUpgrade(BusConfiguration busConfiguration)
        {
            #region 5to6ScanningUpgrade

            IExcludesBuilder excludesBuilder =
                AllAssemblies.Matching("NServiceBus")
                    .And("MyCompany.")
                    .Except("BadAssembly1.dll")
                    .And("BadAssembly2.dll");
            busConfiguration.AssembliesToScan(excludesBuilder);

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