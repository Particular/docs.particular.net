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
        string directoryToProbe = null;

        #region TypeScanningV4

        Configure.With(b => b.AssembliesToScan(listOfAssemblies));

        // or

        Configure.With(b => b.TypesToScan(listOfTypes));

        //or

        Configure.With(b => b.ScanAssembliesInDirectory(directoryToProbe));

        #endregion
    }

}