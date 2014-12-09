using NServiceBus;

public class DataBus
{
    public void Simple()
    {
        string databusPath = null;
        #region FileShareDataBus

        var configure = Configure.With()
            .FileShareDataBus(databusPath);

        #endregion
    }
}