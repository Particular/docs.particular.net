namespace Core3.Scanning
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Reflection;
    using Core3.Handlers;
    using NServiceBus;

    class ScanningPublicApi
    {

        void ScanningListOfAssemblies(IEnumerable<Assembly> myListOfAssemblies, Assembly assembly2, Assembly assembly1)
        {

            #region ScanningListOfAssemblies

            Configure.With(myListOfAssemblies);
            // or
            Configure.With(assembly1, assembly2);

            #endregion
        }

        void ScanningCustomDirectory()
        {
            #region ScanningCustomDirectory

            Configure.With(@"c:\my-custom-dir");

            #endregion
        }

        void ScanningListOfTypes(IEnumerable<Type> myTypes)
        {
            #region ScanningListOfTypes

            Configure.With(myTypes);

            #endregion
        }

        void ScanningExcludeByName()
        {
            #region ScanningExcludeByName

            var allAssemblies = AllAssemblies
                .Except("MyAssembly1.dll")
                .And("MyAssembly2.dll");
            Configure.With(allAssemblies);

            #endregion
        }
        void ScanningExcludeTypes()
        {
            #region ScanningExcludeTypes

            var allTypes = from a in AllAssemblies.Except("Dummy")
                           from t in a.GetTypes()
                           select t;

            var allowedTypesToScan = allTypes
                .Where(t => t != typeof(GenericHandler))
                .ToList();

            Configure.With(allowedTypesToScan);

            #endregion
        }
    }
}