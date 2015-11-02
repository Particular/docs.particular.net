using System;

namespace Snippets6
{
    using NServiceBus;

    public class MinimumConfiguration
    {
        public async void Usage()
        {
            #region MinimumConfiguration

            BusConfiguration busConfiguration = new BusConfiguration();

            #endregion MinimumConfiguration

            #region BusDotCreate

            using (IStartableBus bus = Bus.Create(busConfiguration))
            {
                await bus.StartAsync();
                Console.ReadKey();
            } // Leaving this scope will stop the bus and dispose it.

            #endregion BusDotCreate
        }

    }
}