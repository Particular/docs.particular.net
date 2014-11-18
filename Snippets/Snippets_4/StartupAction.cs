using NServiceBus;

public class StartupAction
{
    public void Simple()
    {
        #region StartupActionV4

        Configure.With().UnicastBus()
            .CreateBus()
            .Start(MyStartupAction);

        #endregion
    }

    public void MyStartupAction()
    {
        
    }
}