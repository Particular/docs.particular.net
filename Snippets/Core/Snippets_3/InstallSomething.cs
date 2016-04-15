namespace Core3
{
    using System.Security.Principal;
    using NServiceBus.Installation;

    #region InstallSomething

    public class MyInstaller : INeedToInstallSomething
    {
        public void Install(WindowsIdentity identity)
        {
            // My code to install something
        }
    }

    #endregion
}