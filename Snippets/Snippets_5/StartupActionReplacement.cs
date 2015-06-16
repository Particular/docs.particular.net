namespace Snippets5
{
    using NServiceBus;

    public class StartupActionReplacement
    {
        public void AllThePersistence()
        {
            #region StartupAction

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