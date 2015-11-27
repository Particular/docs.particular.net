using System.Threading.Tasks;
using NServiceBus.Installation;

public class NeedToInstallSomething :
    IInstall
{

    public Task Install(string identity)
    {
        Logger.WriteLine("Inside IInstall.Install");
        return Task.FromResult(0);
    }
}