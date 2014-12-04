using NServiceBus;

public class StartupAction
{
    public void Simple()
    {
        #region StartupAction

        Configure.With().UnicastBus()
            .CreateBus()
            .Start(MyStartupAction);

        #endregion
    }

    public void MyStartupAction()
    {
        
    }
}