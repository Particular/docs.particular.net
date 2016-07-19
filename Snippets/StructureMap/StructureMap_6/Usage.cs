using NServiceBus;
using StructureMap;

class Usage
{
    Usage(EndpointConfiguration endpointConfiguration)
    {
        #region StructureMap

        endpointConfiguration.UseContainer<StructureMapBuilder>();

        #endregion
    }

    void Existing(EndpointConfiguration endpointConfiguration)
    {
        #region StructureMap_Existing

        var container = new Container(
            action: expression =>
            {
                expression.For<MyService>()
                    .Use(new MyService());
            });
        endpointConfiguration.UseContainer<StructureMapBuilder>(
            customizations: customizations =>
            {
                customizations.ExistingContainer(container);
            });

        #endregion
    }

    class MyService
    {
    }
}