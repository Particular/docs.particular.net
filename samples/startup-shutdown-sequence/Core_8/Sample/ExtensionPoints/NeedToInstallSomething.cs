using System.Threading;
using System.Threading.Tasks;
using NServiceBus.Installation;

public class NeedToInstallSomething :
    INeedToInstallSomething
{
    public Task Install(string identity, CancellationToken token)
    {
        Logger.WriteLine("Inside INeedToInstallSomething.Install");
        return Task.CompletedTask;
    }
}