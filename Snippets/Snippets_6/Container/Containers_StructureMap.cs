namespace Snippets6.Container
{
    using NServiceBus;
    using StructureMap;

    public class Containers_StructureMap
    {
        public void Simple()
        {
            #region StructureMap

            EndpointConfiguration configuration = new EndpointConfiguration();
            configuration.UseContainer<StructureMapBuilder>();

            #endregion
        }

        public void Existing()
        {
            #region StructureMap_Existing

            EndpointConfiguration configuration = new EndpointConfiguration();

            Container container = new Container(x => x.For<MyService>().Use(new MyService()));
            configuration.UseContainer<StructureMapBuilder>(c => c.ExistingContainer(container));

            #endregion
        }

    }
}