namespace Snippets4.Scanning
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

            #region ScanningExcludeByName
            Configure.With(AllAssemblies.Except("MyAssembly1.dll").And("MyAssembly2.dll"));
            #endregion

            #region ScanningListOfTypes
            Configure.With(myTypes);
            #endregion

            #region ScanningListOfAssemblies
            Configure.With(myListOfAssemblies);
            // or
            Configure.With(assembly1, assembly2);
            #endregion

            #region ScanningIncludeByPattern
            Configure.With(AllAssemblies.Matching("NServiceBus").And("MyCompany.").And("SomethingElse"));
            #endregion

            #region ScanningCustomDirectory
            Configure.With(@"c:\my-custom-dir");
            #endregion

            #region ScanningMixingIncludeAndExclude
            Configure.With(AllAssemblies.Matching("NServiceBus").And("MyCompany.").Except("BadAssembly.dll"));
            #endregion

            #region 5to6ScanningUpgrade

            Configure.With(AllAssemblies.Matching("NServiceBus").And("MyCompany.").Except("BadAssembly1.dll").And("BadAssembly2.dll"));

            #endregion
        }

        #region ScanningConfigurationInNSBHost

        public class EndpointConfig : IConfigureThisEndpoint, IWantCustomInitialization
        {
            public void Init()
            {
                // use 'Configure' to configure scanning
            }
        }

        #endregion
    }
}
