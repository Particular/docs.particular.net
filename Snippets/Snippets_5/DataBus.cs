using NServiceBus;

public class DataBus
{
    public void Simple()
    {
        #region FileShareDataBusV5

        var configuration = new BusConfiguration();

        configuration.UseDataBus<FileShareDataBus>();

        #endregion
    }
}