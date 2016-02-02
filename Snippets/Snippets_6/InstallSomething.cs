namespace Snippets4
{
    using System.Threading.Tasks;
    using NServiceBus.Installation;

    #region InstallSomething

    public class MyInstaller : INeedToInstallSomething
    {
        public Task Install(string identity)
        {
            // My code to install something

            return Task.FromResult(0);
        }
    }

    #endregion
}