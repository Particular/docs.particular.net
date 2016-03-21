namespace Snippets6.Container
{
    using NServiceBus;
    using StructureMap;

    class Containers_StructureMap
    {
        Containers_StructureMap(EndpointConfiguration endpointConfiguration)
        {
            #region StructureMap

            endpointConfiguration.UseContainer<StructureMapBuilder>();

            #endregion
        }

        void Existing(EndpointConfiguration endpointConfiguration)
        {
            #region StructureMap_Existing
            
            Container container = new Container(x => x.For<MyService>().Use(new MyService()));
            endpointConfiguration.UseContainer<StructureMapBuilder>(c => c.ExistingContainer(container));

            #endregion
        }

    }
}