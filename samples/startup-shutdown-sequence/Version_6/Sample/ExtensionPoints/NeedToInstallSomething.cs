using System.Threading.Tasks;
using NServiceBus;
using NServiceBus.Installation;

public class NeedToInstallSomething :
    INeedToInstallSomething
{
    public Task InstallAsync(string identity, Configure config)
    {
        Logger.WriteLine("Inside INeedToInstallSomething.Install");
        return Task.FromResult(0);
    }
}