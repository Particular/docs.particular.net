using NServiceBus;
using NServiceBus.Installation;

public class NeedToInstallSomething :
    INeedToInstallSomething
{
    public void Install(string identity, Configure config)
    {
        Logger.WriteLine("Inside INeedToInstallSomething.Install");
    }
}