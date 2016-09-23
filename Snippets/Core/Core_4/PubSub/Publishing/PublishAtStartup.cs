namespace Core4.PubSub.Publishing
{
    using NServiceBus;
    using NServiceBus.Installation.Environments;

    class PublishAtStartup
    {
        public void Publish()
        {
            #region publishAtStartup

            var configure = Configure.With();
            // Other config
            using (var startableBus = configure.UnicastBus().CreateBus())
            {
                var bus = startableBus.Start(
                    startupAction: () =>
                    {
                        configure.ForInstallationOn<Windows>().Install();
                    });
                bus.Publish(new MyEvent());

                #endregion
            }

        }
    }

    public class MyEvent
    {
    }
}