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

            #region ScanningDefault 4
            Configure.With();
            #endregion

            #region ScanningListOfAssemblies 4
            Configure.With(myListOfAssemblies);
            #endregion

            #region ScanningParamArrayOfAssemblies 4
            Configure.With(assembly1, assembly2);
            #endregion

            #region ScanningCustomDirectory 4
            Configure.With(@"c:\my-custom-dir");
            #endregion

            #region ScanningListOfTypes 4
            Configure.With(myTypes);
            #endregion

            #region ScanningExcludeByName 4
            Configure.With(AllAssemblies.Except("MyAssembly.dll").And("MyAssembly.dll"));
            #endregion

            #region ScanningIncludeByPattern 4
            Configure.With(AllAssemblies.Matching("MyCompany.").And("SomethingElse"));
            #endregion

            #region ScanningMixingIncludeAndExclude 4
            Configure.With(AllAssemblies.Matching("MyCompany.").Except("BadAssembly.dll"));
            #endregion
         
            
        }
    }
}