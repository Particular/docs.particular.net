namespace StructureMap_6
{
    using NServiceBus;
    using StructureMap;

    class Usage
    {
        Usage(EndpointConfiguration endpointConfiguration)
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

        class MyService
        {
        }
    }
}