using NServiceBus;

public class FileShareDataBus
{
    public void Simple()
    {
        #region FileShareDataBusV5

        var configuration = new BusConfiguration();

        configuration.FileShareDataBus("path to databus");

        #endregion
    }
}