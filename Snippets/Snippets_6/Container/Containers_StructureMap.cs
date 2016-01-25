namespace Snippets5.Container
{
    using NServiceBus;
    using StructureMap;

    public class Containers_StructureMap
    {
        public void Simple()
        {
            #region StructureMap

            BusConfiguration busConfiguration = new BusConfiguration();
            busConfiguration.UseContainer<StructureMapBuilder>();

            #endregion
        }

        public void Existing()
        {
            #region StructureMap_Existing

            BusConfiguration busConfiguration = new BusConfiguration();

            Container container = new Container(x => x.For<MyService>().Use(new MyService()));
            busConfiguration.UseContainer<StructureMapBuilder>(c => c.ExistingContainer(container));

            #endregion
        }

    }
}