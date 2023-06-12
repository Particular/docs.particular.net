namespace Core8
{
    using System.Threading;
    using System.Threading.Tasks;
    using NServiceBus.Installation;

    #region InstallSomething

    public class MyInstaller :
        INeedToInstallSomething
    {
        public Task Install(string identity, CancellationToken cancellationToken)
        {
            // Code to install something

            return Task.CompletedTask;
        }
    }

    #endregion
}