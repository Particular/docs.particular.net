using System;
using System.Collections.Generic;
using System.Reflection;
using NServiceBus;

public class TypeScanning
{
    public void Simple()
    {
        // Autofac
        var listOfAssemblies = new List<Assembly>();
        var listOfTypes = new List<Type>();
        #region TypeScanningV4

        Configure.With(listOfAssemblies);

        // or

        Configure.With(listOfTypes);

        #endregion
    }

}