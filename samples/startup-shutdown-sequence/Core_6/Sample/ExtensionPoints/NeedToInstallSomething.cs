using System.Threading.Tasks;
using NServiceBus.Installation;

public class NeedToInstallSomething :
    INeedToInstallSomething
{
    public Task Install(string identity)
    {
        Logger.WriteLine("Inside INeedToInstallSomething.Install");
        return Task.FromResult(0);
    }
}