namespace Snippets6.Headers.Writers
{
    using System;
    using System.Collections.Generic;
    using System.Reflection;
    using NServiceBus;

    public static class BusConfigurationExtensions
    {
        public static void SetTypesToScan(this BusConfiguration busConfiguration, IEnumerable<Type> typesToScan)
        {
            MethodInfo methodInfo = typeof(BusConfiguration).GetMethod("TypesToScanInternal",BindingFlags.NonPublic|BindingFlags.Instance);
            methodInfo.Invoke(busConfiguration, new object[]
            {
                typesToScan
            });
        }
    }
}