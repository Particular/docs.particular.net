using NServiceBus;

public class RunCustomActionReplacement
{
    public void Simple()
    {
        #region RunCustomActionReplacementV5

        var bus = Bus.Create(new BusConfiguration());
        MyCustomAction();
        bus.Start();

        #endregion
    }

    public void MyCustomAction()
    {
        
    }
}