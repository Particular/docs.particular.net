namespace Snippets4
{
    using NServiceBus;
    using NServiceBus.Installation;

    #region InstallSomething

    public class MyInstaller : INeedToInstallSomething
    {
        public void Install(string identity, Configure config)
        {
            // My code to install something
        }
    }

    #endregion
}