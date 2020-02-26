using NServiceBus;
using StructureMap;

class Usage
{
    Usage(EndpointConfiguration endpointConfiguration)
    {
        #region StructureMap

        var registry = new Registry();
        registry.For<MyService>().Use(new MyService());

        endpointConfiguration.UseContainer(new StructureMapServiceProviderFactory(registry));

        #endregion
    }

    class MyService
    {
    }
}