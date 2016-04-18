using NServiceBus.Installation;
using NServiceBus.Installation.Environments;

public class NeedToInstallSomething :
    INeedToInstallSomething<Windows>
{
    public void Install(string identity)
    {
        Logger.WriteLine("Inside INeedToInstallSomething.Install");
    }

}