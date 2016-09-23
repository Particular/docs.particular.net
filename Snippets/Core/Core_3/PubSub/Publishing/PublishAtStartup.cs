namespace Core3.PubSub.Publishing
{
    using NServiceBus;
    using NServiceBus.Installation.Environments;

    class PublishAtStartup
    {
        PublishAtStartup(Configure configure)
        {
            #region publishAtStartup

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