using NServiceBus;

public class RunCustomAction
{
    public void Simple()
    {
        // start code RunCustomActionV4

        Configure.With().UnicastBus()
            .RunCustomAction(MyCustomAction)
            .CreateBus()
            .Start();

        // end code RunCustomActionV4
    }

    public void MyCustomAction()
    {
        
    }
}