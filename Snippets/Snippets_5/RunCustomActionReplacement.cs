using NServiceBus;

public class RunCustomActionReplacement
{
    public void Simple()
    {
        #region RunCustomActionReplacementV5

        var configure = Configure.With();
        MyCustomAction();
        configure.CreateBus()
            .Start();

        #endregion
    }

    public void MyCustomAction()
    {
        
    }
}