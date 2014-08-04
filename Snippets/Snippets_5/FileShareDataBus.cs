using NServiceBus;

public class FileShareDataBus
{
    public void Simple()
    {
        string databusPath = null;
        // start code FileShareDataBusV5

        var configure = Configure.With(b => b.FileShareDataBus(databusPath));

        // end code FileShareDataBusV5
    }
}