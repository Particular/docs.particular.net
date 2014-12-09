using NServiceBus;

public class MinimumConfiguration
{
    public MinimumConfiguration()
    {
        #region MinimumConfiguration

        var configuration = new BusConfiguration();

        #endregion MinimumConfiguration

        #region BusDotCreate

        using (var bus = Bus.Create(configuration))
        {
            bus.Start();
        }

        #endregion BusDotCreate
    }

}