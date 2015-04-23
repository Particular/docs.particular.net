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

        #region ScanningListOfAssemblies

        busConfiguration.AssembliesToScan(myListOfAssemblies);

        #endregion

        #region ScanningParamArrayOfAssemblies

        busConfiguration.AssembliesToScan(assembly1, assembly2);

        #endregion

        #region ScanningCustomDirectory

        busConfiguration.ScanAssembliesInDirectory(@"c:\my-custom-dir");

        #endregion

        #region ScanningListOfTypes

        busConfiguration.TypesToScan(myTypes);

        #endregion

        #region ScanningExcludeByName

        busConfiguration.AssembliesToScan(AllAssemblies.Except("MyAssembly.dll").And("MyAssembly.dll"));

        #endregion

        #region ScanningIncludeByPattern

        busConfiguration.AssembliesToScan(AllAssemblies.Matching("MyCompany.").And("SomethingElse"));

        #endregion

        #region ScanningMixingIncludeAndExclude

        busConfiguration.AssembliesToScan(AllAssemblies.Matching("MyCompany.").Except("BadAssembly.dll"));

        #endregion

    }
}