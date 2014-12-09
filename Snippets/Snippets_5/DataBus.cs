using NServiceBus;

public class DataBus
{
    public void Simple()
    {
        #region FileShareDataBus

        var configuration = new BusConfiguration();

        configuration.UseDataBus<FileShareDataBus>();

        #endregion
    }
}