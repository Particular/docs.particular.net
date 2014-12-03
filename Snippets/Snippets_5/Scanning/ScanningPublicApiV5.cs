using System;
using System.Collections.Generic;
using System.Reflection;
using NServiceBus;

public class ScanningPublicApiV5
{
    public ScanningPublicApiV5()
    {
        IEnumerable<Assembly> myListOfAssemblies = null;

        Assembly assembly2 = null;
        Assembly assembly1 = null;

        IEnumerable<Type> myTypes = null;


        #region ScanningDefault

        var configuration = new BusConfiguration();

        #endregion

        #region ScanningListOfAssemblies

        configuration.AssembliesToScan(myListOfAssemblies);

        #endregion

        #region ScanningParamArrayOfAssemblies

        configuration.AssembliesToScan(assembly1, assembly2);

        #endregion

        #region ScanningCustomDirectory

        configuration.ScanAssembliesInDirectory(@"c:\my-custom-dir");

        #endregion

        #region ScanningListOfTypes

        configuration.TypesToScan(myTypes);

        #endregion

        #region ScanningExcludeByName

        configuration.AssembliesToScan(AllAssemblies.Except("MyAssembly.dll").And("MyAssembly.dll"));

        #endregion

        #region ScanningIncludeByPattern

        configuration.AssembliesToScan(AllAssemblies.Matching("MyCompany.").And("SomethingElse"));

        #endregion

        #region ScanningMixingIncludeAndExclude

        configuration.AssembliesToScan(AllAssemblies.Matching("MyCompany.").Except("BadAssembly.dll"));

        #endregion

    }
}