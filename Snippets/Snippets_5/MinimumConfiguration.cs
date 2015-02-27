using NServiceBus;

public class MinimumConfiguration
{
    public MinimumConfiguration()
    {
        #region MinimumConfiguration

        BusConfiguration configuration = new BusConfiguration();

        #endregion MinimumConfiguration

        #region BusDotCreate

        using (IStartableBus bus = Bus.Create(configuration))
        {
            bus.Start();
        }

        #endregion BusDotCreate
    }

}