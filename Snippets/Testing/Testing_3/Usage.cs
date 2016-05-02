using System.Reflection;
using NServiceBus.Testing;

class Usage
{
    void InitializeAssemblies()
    {
        #region TestInitializeAssemblies

        Assembly[] assembliesToScan =
        {
            typeof(HandlerToTest).Assembly,
            Assembly.LoadFrom("NServiceBus.Testing.dll")
        };
        Test.Initialize(assembliesToScan);

        #endregion
    }

    class HandlerToTest
    {
    }
}