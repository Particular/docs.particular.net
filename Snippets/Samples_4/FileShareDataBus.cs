using NServiceBus;

public class FileShareDataBus
{
    public void Simple()
    {
        string databusPath = null;
        // start code FileShareDataBusV4

        var configure = Configure.With()
            .FileShareDataBus(databusPath);

        // end code FileShareDataBusV4
    }
}