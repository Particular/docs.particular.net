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

        #region TypeScanningV5

        var configuration = new BusConfiguration();

        configuration.AssembliesToScan(listOfAssemblies);

        // or
        configuration.TypesToScan(listOfTypes);

        //or
        configuration.ScanAssembliesInDirectory(directoryToProbe);

        #endregion
    }

}