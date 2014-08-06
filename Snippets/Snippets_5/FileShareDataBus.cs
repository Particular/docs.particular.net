using NServiceBus;

public class FileShareDataBus
{
    public void Simple()
    {
        var databusPath = "";

        #region FileShareDataBusV5

        var configure = Configure.With(b => b.FileShareDataBus(databusPath));

        #endregion
    }
}