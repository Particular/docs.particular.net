using NServiceBus;

public class StartupActionReplacement
{
    public void AllThePersistence()
    {
        #region StartupAction

        var bus = Bus.Create(new BusConfiguration());
        MyCustomAction();
        bus.Start();

        #endregion
    }

    public void MyCustomAction()
    {
        
    }
}