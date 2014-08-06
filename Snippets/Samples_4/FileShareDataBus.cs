using NServiceBus;

public class FileShareDataBus
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