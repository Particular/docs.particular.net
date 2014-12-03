namespace MyServer.Scanning
{
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
            Configure.With();
            #endregion

            #region ScanningListOfAssemblies
            Configure.With(myListOfAssemblies);
            #endregion

            #region ScanningParamArrayOfAssemblies
            Configure.With(assembly1, assembly2);
            #endregion

            #region ScanningCustomDirectory
            Configure.With(@"c:\my-custom-dir");
            #endregion

            #region ScanningListOfTypes
            Configure.With(myTypes);
            #endregion

            #region ScanningExcludeByName
            Configure.With(AllAssemblies.Except("MyAssembly.dll").And("MyAssembly.dll"));
            #endregion

            #region ScanningIncludeByPattern
            Configure.With(AllAssemblies.Matching("MyCompany.").And("SomethingElse"));
            #endregion

            #region ScanningMixingIncludeAndExclude
            Configure.With(AllAssemblies.Matching("MyCompany.").Except("BadAssembly.dll"));
            #endregion
         
            
        }
    }
}