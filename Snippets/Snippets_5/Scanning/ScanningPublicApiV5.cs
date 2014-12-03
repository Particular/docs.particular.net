namespace MyServer.Scanning
{
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


            #region ScanningDefault 5
            var configuration = new BusConfiguration();
            #endregion

            #region ScanningListOfAssemblies 5
            configuration.AssembliesToScan(myListOfAssemblies);
            #endregion

            #region ScanningParamArrayOfAssemblies 5
            configuration.AssembliesToScan(assembly1, assembly2);
            #endregion

            #region ScanningCustomDirectory 5
            configuration.ScanAssembliesInDirectory(@"c:\my-custom-dir");
            #endregion

            #region ScanningListOfTypes 5
            configuration.TypesToScan(myTypes);
            #endregion

            #region ScanningExcludeByName 5
            configuration.AssembliesToScan(AllAssemblies.Except("MyAssembly.dll").And("MyAssembly.dll"));
            #endregion

            #region ScanningIncludeByPattern 5
            configuration.AssembliesToScan(AllAssemblies.Matching("MyCompany.").And("SomethingElse"));
            #endregion

            #region ScanningMixingIncludeAndExclude 5
            configuration.AssembliesToScan(AllAssemblies.Matching("MyCompany.").Except("BadAssembly.dll"));
            #endregion

        }
    }
}