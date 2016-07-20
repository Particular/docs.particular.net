using System.Collections.Generic;
using System.Reflection;
using NServiceBus.Testing;

class Usage
{
    void InitializeAssemblies()
    {
        #region TestInitializeAssemblies

        var assembliesToScan = new List<Assembly>
        {
            typeof(HandlerToTest).Assembly,
            Assembly.LoadFrom("NServiceBus.Testing.dll")
        };
        Test.Initialize(
            customisations: configuration =>
            {
                configuration.AssembliesToScan(assembliesToScan);
            });

        #endregion
    }

    class HandlerToTest
    {
    }
}