using NServiceBus;
using StructureMap;

class Usage
{
    Usage(EndpointConfiguration endpointConfiguration)
    {
#pragma warning disable CS0618 // Type or member is obsolete
        #region StructureMap

        endpointConfiguration.UseContainer<StructureMapBuilder>();

        #endregion
#pragma warning restore CS0618 // Type or member is obsolete
    }

    void Existing(EndpointConfiguration endpointConfiguration)
    {
#pragma warning disable CS0618 // Type or member is obsolete
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
#pragma warning restore CS0618 // Type or member is obsolete
    }

    class MyService
    {
    }
}