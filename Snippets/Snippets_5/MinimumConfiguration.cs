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
        }

        #endregion BusDotCreate
    }

}