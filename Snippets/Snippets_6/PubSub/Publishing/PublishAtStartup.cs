namespace Snippets5.PubSub.Publishing
{
    using System.Threading.Tasks;
    using NServiceBus;

    class PublishAtStartup
    {
        public async Task Publish()
        {
            #region publishAtStartup

            BusConfiguration busConfiguration = new BusConfiguration();
            //Other config
            using (IBus bus = Bus.Create(busConfiguration).Start())
            {
                await bus.PublishAsync(new MyEvent());

                #endregion
            }

        }
    }

    public class MyEvent
    {
    }
}