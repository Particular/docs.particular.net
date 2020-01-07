using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using NServiceBus;
using Unity;
using Unity.Extension;

static class Program
{
    static async Task Main()
    {
        Console.Title = "Samples.Unity";

        #region ContainerConfiguration

        var endpointConfiguration = new EndpointConfiguration("Samples.Unity");
        var container = new UnityContainer();
        container.RegisterInstance(new MyService());
        endpointConfiguration.UseContainer<UnityBuilder>(
            customizations: customizations =>
            {
                customizations.UseExistingContainer(container);
            });

        #endregion

        var extensionsField = typeof(UnityContainer).GetField("_extensions", BindingFlags.GetField | BindingFlags.NonPublic | BindingFlags.Instance);
        var extensions = (List<IUnityContainerExtensionConfigurator>)extensionsField.GetValue(container);
        foreach (var extension in extensions.ToList())
        {
            if (extension.GetType().FullName == "NServiceBus.Unity.PropertyInjectionContainerExtension")
            {
                extensions.Remove(extension);
                break;
            }
        }

        endpointConfiguration.UsePersistence<LearningPersistence>();
        endpointConfiguration.UseTransport<LearningTransport>();

        var endpointInstance = await Endpoint.Start(endpointConfiguration)
            .ConfigureAwait(false);
        var myMessage = new MyMessage();
        await endpointInstance.SendLocal(myMessage)
            .ConfigureAwait(false);
        Console.WriteLine("Press any key to exit");
        Console.ReadKey();
        await endpointInstance.Stop()
            .ConfigureAwait(false);
    }
}