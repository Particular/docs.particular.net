using NServiceBus;
using NServiceBus.Features;

namespace Core;

using System.Threading;
using System.Threading.Tasks;
using NServiceBus.Installation;

#region InstallSomething

public class MyInstaller : INeedToInstallSomething
{
    public Task Install(string identity, CancellationToken cancellationToken)
    {
        // Code to install something

        return Task.CompletedTask;
    }
}

#endregion

class ShowInstallation
{
    public void Snippet(EndpointConfiguration endpointConfiguration)
    {
#region AddInstallerFromConfig
        endpointConfiguration.AddInstaller<MyInstaller>();
#endregion
    }
}

#region AddInstallerFromFeature

public class MyFeature : Feature
{
    protected override void Setup(FeatureConfigurationContext context)
    {
        context.AddInstaller<MyInstaller>();
    }
}

#endregion