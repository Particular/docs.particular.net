using NServiceBus;

public class RunCustomActionReplacement
{
    public void Simple()
    {
        // start code RunCustomActionReplacementV5

        var configure = Configure.With();
        MyCustomAction();
        configure.CreateBus()
            .Start();

        // end code RunCustomActionReplacementV5
    }

    public void MyCustomAction()
    {
        
    }
}