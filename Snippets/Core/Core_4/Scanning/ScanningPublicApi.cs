﻿namespace Core4.Scanning
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Reflection;
    using Core4.Handlers;
    using NServiceBus;

    class ScanningPublicApi
    {
        void ScanningDefault()
        {
            #region ScanningDefault

            Configure.With();

            #endregion

        }

        void ScanningExcludeByName()
        {
            #region ScanningExcludeByName

            var excludesBuilder = AllAssemblies
                .Except("MyAssembly1.dll")
                .And("MyAssembly2.dll");
            Configure.With(excludesBuilder);

            #endregion
        }

        void ScanningListOfTypes(IEnumerable<Type> myTypes)
        {
            #region ScanningListOfTypes

            Configure.With(myTypes);

            #endregion

        }

        void ScanningListOfAssemblies(IEnumerable<Assembly> myListOfAssemblies, Assembly assembly2, Assembly assembly1)
        {
            #region ScanningListOfAssemblies

            Configure.With(myListOfAssemblies);
            // or
            Configure.With(assembly1, assembly2);

            #endregion

        }

        void ScanningIncludeByPattern()
        {
            #region ScanningIncludeByPattern

            var includesBuilder = AllAssemblies
                .Matching("NServiceBus")
                .And("MyCompany.")
                .And("SomethingElse");
            Configure.With(includesBuilder);

            #endregion
        }

        void ScanningCustomDirectory()
        {
            #region ScanningCustomDirectory

            Configure.With(@"c:\my-custom-dir");

            #endregion

        }

        void ScanningMixingIncludeAndExclude()
        {
            #region ScanningMixingIncludeAndExclude

            var excludesBuilder = AllAssemblies
                .Matching("NServiceBus")
                .And("MyCompany.")
                .Except("BadAssembly.dll");
            Configure.With(excludesBuilder);

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