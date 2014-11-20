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
            var configuration = new BusConfiguration();
            IEnumerable<Assembly> myListOfAssemblies = null;

            Assembly assembly2 = null;
            Assembly assembly1 = null;

            IEnumerable<Type> myTypes = null;


            #region ScanningListOfAssembliesV5
            configuration.AssembliesToScan(myListOfAssemblies);
            #endregion

            #region ScanningParamArrayOfAssembliesV5
            configuration.AssembliesToScan(assembly1, assembly2);
            #endregion


            #region ScanningCustomDirectoryV5
            configuration.ScanAssembliesInDirectory(@"c:\my-custom-dir");
            #endregion


            #region ScanningListOfTypesV5
            configuration.TypesToScan(myTypes);
            #endregion

            #region ScanningExcludeByNameV5
            configuration.AssembliesToScan(AllAssemblies.Except("MyAssembly.dll").And("MyAssembly.dll"));
            #endregion

            #region ScanningIncludeByPatternV5
            configuration.AssembliesToScan(AllAssemblies.Matching("MyCompany.").And("SomethingElse"));
            #endregion


            #region ScanningMixingIncludeAndExcludeV5
            configuration.AssembliesToScan(AllAssemblies.Matching("MyCompany.").Except("BadAssembly.dll"));
            #endregion

            
            

        }
    }
}