namespace Snippets3.Container
{
    using NServiceBus;
    using NServiceBus.ObjectBuilder.Common.Config;
    using NServiceBus.ObjectBuilder.StructureMap;
    using StructureMap;

    public class Containers_StructureMap
    {
        public void Simple()
        {
            #region StructureMap

            Configure.With()
                .UsingContainer<StructureMapObjectBuilder>();

            #endregion
        }

        public void Existing()
        {
            IContainer structureMapContainer = null;

            #region StructureMap_Existing

            Configure.With()
                .UsingContainer(new StructureMapObjectBuilder(structureMapContainer));

            #endregion
        }

    }
}