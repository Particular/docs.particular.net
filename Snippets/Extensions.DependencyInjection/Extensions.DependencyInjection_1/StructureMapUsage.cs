using NServiceBus;
using StructureMap;

class StructureMapUsage
{
    StructureMapUsage(EndpointConfiguration endpointConfiguration)
    {
        #region StructureMapUsage

        var registry = new Registry();
        registry.For<MyService>().Use(new MyService());

        endpointConfiguration.UseContainer(new StructureMapServiceProviderFactory(registry));

        #endregion
    }

    class MyService
    {
    }
}