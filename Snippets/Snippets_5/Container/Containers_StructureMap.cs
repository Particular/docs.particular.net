namespace Snippets5.Container
{
    using NServiceBus;

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
            StructureMap.IContainer structureMapContainer = null;

            #region StructureMap_Existing

            BusConfiguration busConfiguration = new BusConfiguration();
            busConfiguration.UseContainer<StructureMapBuilder>(c => c.ExistingContainer(structureMapContainer));

            #endregion
        }

    }
}