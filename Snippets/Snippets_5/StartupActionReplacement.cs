using NServiceBus;

public class StartupActionReplacement
{
    public void AllThePersistence()
    {
        // start code StartupActionReplacementV5

        var configure = Configure.With();
        var bus = configure.CreateBus();
        MyCustomAction();
        bus.Start();

        // end code StartupActionReplacementV5
    }

    public void MyCustomAction()
    {
        
    }
}