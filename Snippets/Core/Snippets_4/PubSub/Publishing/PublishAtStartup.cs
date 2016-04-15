namespace Core4.PubSub.Publishing
{
    using NServiceBus;
    using NServiceBus.Installation.Environments;

    class PublishAtStartup
    {
        public void Publish()
        {
            #region publishAtStartup

            Configure configure = Configure.With();
            //Other config
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
