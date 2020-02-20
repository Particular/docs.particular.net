using NServiceBus;
using StructureMap;

class Usage
{
    Usage(BusConfiguration busConfiguration)
    {
        #region StructureMap

        busConfiguration.UseContainer<StructureMapBuilder>();

        #endregion
    }

    void Existing(BusConfiguration busConfiguration)
    {
        #region StructureMap_Existing

        var container = new Container(
            action: expression =>
            {
                expression.For<MyService>()
                    .Use(new MyService());
            });
        busConfiguration.UseContainer<StructureMapBuilder>(
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