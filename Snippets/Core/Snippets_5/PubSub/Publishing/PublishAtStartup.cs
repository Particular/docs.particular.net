namespace Core5.PubSub.Publishing
{
    using NServiceBus;

    class PublishAtStartup
    {
        void Publish(BusConfiguration busConfiguration)
        {
            #region publishAtStartup

            using (IBus bus = Bus.Create(busConfiguration).Start())
            {
                bus.Publish(new MyEvent());
                #endregion
            }

        }
    }

    class MyEvent
    {
    }
}