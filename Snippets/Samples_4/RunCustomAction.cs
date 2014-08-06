using NServiceBus;

public class RunCustomAction
{
    public void Simple()
    {
        #region RunCustomActionV4

        Configure.With().UnicastBus()
            .RunCustomAction(MyCustomAction)
            .CreateBus()
            .Start();

        #endregion
    }

    public void MyCustomAction()
    {
        
    }
}