namespace Core4
{
    using NServiceBus.Installation;

    #region InstallSomething

    public class MyInstaller :
        INeedToInstallSomething
    {
        public void Install(string identity)
        {
            // Code to install something
        }
    }

    #endregion
}