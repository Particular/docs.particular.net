namespace Core3.PubSub.Publishing
{
    using NServiceBus;
    using NServiceBus.Installation.Environments;

    class PublishAtStartup
    {
        PublishAtStartup(Configure configure)
        {
            #region publishAtStartup
            using (IStartableBus startableBus = configure.UnicastBus().CreateBus())
            {
                IBus bus = startableBus.Start(() => configure.ForInstallationOn<Windows>().Install());
                bus.Publish(new MyEvent());

                #endregion
            }

        }
    }

    public class MyEvent { }
}
