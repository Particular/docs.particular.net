namespace Snippets4.Container
{
    using NServiceBus;
    using StructureMap;

    class Containers_StructureMap
    {
        void Simple(Configure configure)
        {
            #region StructureMap

            configure.StructureMapBuilder();

            #endregion
        }

        void Existing(Configure configure)
        {
            #region StructureMap_Existing

            Container container = new Container(x => x.For<MyService>().Use(new MyService()));
            configure.StructureMapBuilder(container);

            #endregion
        }

    }
}