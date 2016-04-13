namespace StructureMap_4
{
    using NServiceBus;
    using StructureMap;

    class Usage
    {
        Usage(Configure configure)
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

        class MyService
        {
        }
    }
}