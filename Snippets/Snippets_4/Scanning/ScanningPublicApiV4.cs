namespace MyServer.Scanning
{
    using System;
    using System.Collections.Generic;
    using System.Reflection;
    using NServiceBus;

    public class ScanningPublicApiV4
    {
        public ScanningPublicApiV4()
        {
           IEnumerable<Assembly> myListOfAssemblies = null;

            Assembly assembly2 = null;
            Assembly assembly1 = null;

            IEnumerable<Type> myTypes = null;

            #region ScanningDefaultV4
            Configure.With();
            #endregion


            #region ScanningListOfAssembliesV4
            Configure.With(myListOfAssemblies);
            #endregion

            #region ScanningParamArrayOfAssembliesV4
            Configure.With(assembly1, assembly2);
            #endregion


            #region ScanningCustomDirectoryV4
            Configure.With(@"c:\my-custom-dir");
            #endregion


            #region ScanningListOfTypesV4
            Configure.With(myTypes);
            #endregion

            #region ScanningExcludeByNameV4
            Configure.With(AllAssemblies.Except("MyAssembly.dll").And("MyAssembly.dll"));
            #endregion

            #region ScanningIncludeByPatternV4
            Configure.With(AllAssemblies.Matching("MyCompany.").And("SomethingElse"));
            #endregion


            #region ScanningMixingIncludeAndExcludeV4
            Configure.With(AllAssemblies.Matching("MyCompany.").Except("BadAssembly.dll"));
            #endregion
         
            
        }
    }
}