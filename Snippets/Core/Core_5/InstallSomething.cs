namespace Core5
{
    using NServiceBus;
    using NServiceBus.Installation;

    #region InstallSomething

    public class MyInstaller :
        INeedToInstallSomething
    {
        public void Install(string identity, Configure config)
        {
            // Code to install something
        }
    }

    #endregion
}