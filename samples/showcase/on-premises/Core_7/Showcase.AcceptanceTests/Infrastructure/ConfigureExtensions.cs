namespace Showcase.AcceptanceTests
{
    using NServiceBus;
    using NServiceBus.AcceptanceTesting.Support;
    using NServiceBus.Configuration.AdvancedExtensibility;
    using NServiceBus.ObjectBuilder;

    public static class ConfigureExtensions
    {
        // NOTE: There is no way out of the box to get at routing data
        public static RoutingSettings ConfigureRouting(this EndpointConfiguration configuration) =>
            new RoutingSettings(configuration.GetSettings());

        // NOTE: This allows spy handlers and components to accept a dependency on the scenario context
        public static void RegisterComponentsAndInheritanceHierarchy(this EndpointConfiguration builder, RunDescriptor runDescriptor)
        {
            builder.RegisterComponents(r => { RegisterInheritanceHierarchyOfContextOnContainer(runDescriptor, r); });
        }

        static void RegisterInheritanceHierarchyOfContextOnContainer(RunDescriptor runDescriptor, IConfigureComponents r)
        {
            var type = runDescriptor.ScenarioContext.GetType();
            while (type != typeof(object))
            {
                r.RegisterSingleton(type, runDescriptor.ScenarioContext);
                type = type.BaseType;
            }
        }
    }
}