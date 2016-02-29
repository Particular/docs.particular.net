namespace Snippets6.Container
{
    using NServiceBus;
    using StructureMap;

    public class Containers_StructureMap
    {
        public void Simple()
        {
            #region StructureMap

            EndpointConfiguration endpointConfiguration = new EndpointConfiguration();
            endpointConfiguration.UseContainer<StructureMapBuilder>();

            #endregion
        }

        public void Existing()
        {
            #region StructureMap_Existing

            EndpointConfiguration endpointConfiguration = new EndpointConfiguration();

            Container container = new Container(x => x.For<MyService>().Use(new MyService()));
            endpointConfiguration.UseContainer<StructureMapBuilder>(c => c.ExistingContainer(container));

            #endregion
        }

    }
}