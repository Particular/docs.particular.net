using System;

namespace Snippets5
{
    using NServiceBus;

    public class MinimumConfiguration
    {
        public MinimumConfiguration()
        {
            #region MinimumConfiguration

            BusConfiguration busConfiguration = new BusConfiguration();

            #endregion MinimumConfiguration

            #region BusDotCreate

            using (IStartableBus bus = Bus.Create(busConfiguration))
            {
                bus.Start();
                Console.ReadKey();
            } // Leaving this scope will stop the bus and dispose it.

            #endregion BusDotCreate
        }

    }
}