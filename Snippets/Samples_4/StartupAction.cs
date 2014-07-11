using NServiceBus;

public class StartupAction
{
    public void Simple()
    {
        // start code StartupActionV4

        Configure.With().UnicastBus()
            .CreateBus()
            .Start(MyStartupAction);

        // end code StartupActionV4
    }

    public void MyStartupAction()
    {
        
    }
}