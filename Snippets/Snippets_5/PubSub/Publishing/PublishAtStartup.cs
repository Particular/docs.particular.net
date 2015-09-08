namespace Snippets5.PubSub.Publishing
{
    using NServiceBus;

    class PublishAtStartup
    {
        public void Publish()
        {
            #region publishAtStartup

            BusConfiguration busConfiguration = new BusConfiguration();
            //Other config
            using (IBus bus = Bus.Create(busConfiguration).Start())
            {
                bus.Publish(new MyEvent());
                #endregion
            }

        }
    }

    public class MyEvent
    {
    }
}