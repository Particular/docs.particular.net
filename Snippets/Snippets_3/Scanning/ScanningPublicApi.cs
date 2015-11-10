namespace Snippets3.Scanning
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

            Configure.With();

            #endregion
        }

        public void ScanningListOfAssemblies()
        {
            IEnumerable<Assembly> myListOfAssemblies = null;

            Assembly assembly2 = null;
            Assembly assembly1 = null;

            #region ScanningListOfAssemblies

            Configure.With(myListOfAssemblies);
            // or
            Configure.With(assembly1, assembly2);

            #endregion
        }

        public void ScanningCustomDirectory()
        {
            #region ScanningCustomDirectory

            Configure.With(@"c:\my-custom-dir");

            #endregion
        }

        public void ScanningListOfTypes()
        {
            IEnumerable<Type> myTypes = null;

            #region ScanningListOfTypes

            Configure.With(myTypes);

            #endregion
        }

        public void ScanningExcludeByName()
        {
            #region ScanningExcludeByName

            AllAssemblies allAssemblies = AllAssemblies
                .Except("MyAssembly1.dll")
                .And("MyAssembly2.dll");
            Configure.With(allAssemblies);

            #endregion
        }
    }
}