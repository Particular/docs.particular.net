using System.Security.Principal;
using NServiceBus.Installation;
using NServiceBus.Installation.Environments;

public class NeedToInstallSomething :
    INeedToInstallSomething<Windows>
{
    public void Install(WindowsIdentity identity)
    {
        Logger.WriteLine("Inside INeedToInstallSomething.Install");
    }
}