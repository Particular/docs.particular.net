namespace Snippets6.Container
{
    using NServiceBus;
    using StructureMap;

    public class Containers_StructureMap
    {
        public void Simple(EndpointConfiguration endpointConfiguration)
        {
            #region StructureMap

            endpointConfiguration.UseContainer<StructureMapBuilder>();

            #endregion
        }

        public void Existing(EndpointConfiguration endpointConfiguration)
        {
            #region StructureMap_Existing


            Container container = new Container(x => x.For<MyService>().Use(new MyService()));
            endpointConfiguration.UseContainer<StructureMapBuilder>(c => c.ExistingContainer(container));

            #endregion
        }

    }
}