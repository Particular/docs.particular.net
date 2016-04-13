namespace Snippets5.UnitTesting
{
    using System.Collections.Generic;
    using System.Reflection;
    using NServiceBus.Testing;

    class Usage
    {
        void InitializeAssemblies()
        {
            #region TestInitializeAssemblies

            List<Assembly> assembliesToScan = new List<Assembly>
            {
                typeof(HandlerToTest).Assembly,
                Assembly.LoadFrom("NServiceBus.Testing.dll")
            };
            Test.Initialize(busConfiguration =>
            {
                busConfiguration.AssembliesToScan(assembliesToScan);
            });

            #endregion
        }

        class HandlerToTest
        {
        }
    }

}