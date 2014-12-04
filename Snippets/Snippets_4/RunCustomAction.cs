using NServiceBus;

public class RunCustomAction
{
    public void Simple()
    {
        #region RunCustomAction

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