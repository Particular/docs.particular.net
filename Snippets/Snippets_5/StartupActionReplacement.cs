using NServiceBus;

public class StartupActionReplacement
{
    public void AllThePersistence()
    {
        #region StartupActionReplacementV5

        var configure = Configure.With();
        var bus = configure.CreateBus();
        MyCustomAction();
        bus.Start();

        #endregion
    }

    public void MyCustomAction()
    {
        
    }
}