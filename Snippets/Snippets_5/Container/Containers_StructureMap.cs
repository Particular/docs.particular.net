namespace Snippets5.Container
{
    using NServiceBus;
    using StructureMap;

    public class Containers_StructureMap
    {
        public void Simple()
        {
            BusConfiguration busConfiguration = new BusConfiguration();
            #region StructureMap

            busConfiguration.UseContainer<StructureMapBuilder>();

            #endregion
        }

        public void Existing()
        {
            BusConfiguration busConfiguration = new BusConfiguration();
            #region StructureMap_Existing


            Container container = new Container(x => x.For<MyService>().Use(new MyService()));
            busConfiguration.UseContainer<StructureMapBuilder>(c => c.ExistingContainer(container));

            #endregion
        }

    }
}