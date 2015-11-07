namespace Snippets5.Scanning
{
    using System;
    using System.Collections.Generic;
    using System.Reflection;
    using NServiceBus;

    public class ScanningPublicApi
    {
        public void ScanningDefault()
        {
            #region ScanningDefault

            BusConfiguration busConfiguration = new BusConfiguration();

            #endregion
        }

        public void ScanningExcludeByName()
        {
            BusConfiguration busConfiguration = new BusConfiguration();

            #region ScanningExcludeByName

            IExcludesBuilder excludesBuilder = AllAssemblies
                .Except("MyAssembly1.dll")
                .And("MyAssembly2.dll");
            busConfiguration.AssembliesToScan(excludesBuilder);

            #endregion
        }

        public void ScanningListOfTypes()
        {
            IEnumerable<Type> myTypes = null;
            BusConfiguration busConfiguration = new BusConfiguration();

            #region ScanningListOfTypes

            busConfiguration.TypesToScan(myTypes);

            #endregion
        }

        public void ScanningListOfAssemblies()
        {
            IEnumerable<Assembly> myListOfAssemblies = null;

            Assembly assembly2 = null;
            Assembly assembly1 = null;
            BusConfiguration busConfiguration = new BusConfiguration();

            #region ScanningListOfAssemblies

            busConfiguration.AssembliesToScan(myListOfAssemblies);
            // or
            busConfiguration.AssembliesToScan(assembly1, assembly2);

            #endregion
        }

        public void ScanningIncludeByPattern()
        {
            BusConfiguration busConfiguration = new BusConfiguration();

            #region ScanningIncludeByPattern

            IIncludesBuilder includesBuilder = AllAssemblies
                .Matching("NServiceBus")
                .And("MyCompany.")
                .And("SomethingElse");
            busConfiguration.AssembliesToScan(includesBuilder);

            #endregion
        }

        public void ScanningCustomDirectory()
        {
            BusConfiguration busConfiguration = new BusConfiguration();

            #region ScanningCustomDirectory

            busConfiguration.ScanAssembliesInDirectory(@"c:\my-custom-dir");

            #endregion
        }

        public void ScanningMixingIncludeAndExclude()
        {
            BusConfiguration busConfiguration = new BusConfiguration();

            #region ScanningMixingIncludeAndExclude

            IExcludesBuilder excludesBuilder = AllAssemblies
                .Matching("NServiceBus")
                .And("MyCompany.")
                .Except("BadAssembly.dll");
            busConfiguration.AssembliesToScan(excludesBuilder);

            #endregion

        }

        public void ScanningUpgrade()
        {
            BusConfiguration busConfiguration = new BusConfiguration();

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