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


        Container container = new Container(x => x.For<MyService>().Use(new MyService()));
        busConfiguration.UseContainer<StructureMapBuilder>(c => c.ExistingContainer(container));

        #endregion
    }

    class MyService
    {
    }
}