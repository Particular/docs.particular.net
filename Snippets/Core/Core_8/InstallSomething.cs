namespace Core7
{
    using System.Threading.Tasks;
    using NServiceBus.Installation;

    #region InstallSomething

    public class MyInstaller :
        INeedToInstallSomething
    {
        public Task Install(string identity)
        {
            // Code to install something

            return Task.CompletedTask;
        }
    }

    #endregion
}