using System;
using System.Collections.Generic;
using System.Reflection;
using NServiceBus;

public class ScanningPublicApi
{
    public ScanningPublicApi()
    {
        IEnumerable<Assembly> myListOfAssemblies = null;

        Assembly assembly2 = null;
        Assembly assembly1 = null; 

        IEnumerable<Type> myTypes = null;


        #region ScanningDefault

        BusConfiguration busConfiguration = new BusConfiguration();

        #endregion

        #region ScanningExcludeByName

        busConfiguration.AssembliesToScan(AllAssemblies.Except("MyAssembly1.dll").And("MyAssembly2.dll"));

        #endregion

        #region ScanningListOfTypes

        busConfiguration.TypesToScan(myTypes); 

        #endregion

        #region ScanningListOfAssemblies

        busConfiguration.AssembliesToScan(myListOfAssemblies);
        // or
        busConfiguration.AssembliesToScan(assembly1, assembly2);

        #endregion

        #region ScanningIncludeByPattern

        busConfiguration.AssembliesToScan(AllAssemblies.Matching("MyCompany.").And("SomethingElse"));

        #endregion

        #region ScanningCustomDirectory

        busConfiguration.ScanAssembliesInDirectory(@"c:\my-custom-dir");

        #endregion

        #region ScanningMixingIncludeAndExclude

        busConfiguration.AssembliesToScan(AllAssemblies.Matching("MyCompany.").Except("BadAssembly.dll"));

        #endregion

    }

    #region ScanningConfigurationInNSBHost

    public class EndpointConfig : IConfigureThisEndpoint
    {
        public void Customize(BusConfiguration configuration)
        {
            // use 'configuration' object to configure scanning
        }
    }

    #endregion

}