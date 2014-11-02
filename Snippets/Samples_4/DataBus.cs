using NServiceBus;

public class DataBus
{
    public void Simple()
    {
        string databusPath = null;
        #region FileShareDataBusV4

        var configure = Configure.With()
            .FileShareDataBus(databusPath);

        #endregion
    }
}