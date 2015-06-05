namespace Snippets5
{
    using NServiceBus;

    public class RunCustomActionReplacement
    {
        public void Simple()
        {
            #region RunCustomAction

            IStartableBus bus = Bus.Create(new BusConfiguration());
            MyCustomAction();
            bus.Start();

            #endregion
        }

        public void MyCustomAction()
        {
        
        }
    }
}