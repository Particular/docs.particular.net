using NServiceBus;

public class StartupActionReplacement
{
    public void AllThePersistence()
    {
        #region StartupActionReplacementV5

        var bus = Bus.Create(new BusConfiguration());
        MyCustomAction();
        bus.Start();

        #endregion
    }

    public void MyCustomAction()
    {
        
    }
}