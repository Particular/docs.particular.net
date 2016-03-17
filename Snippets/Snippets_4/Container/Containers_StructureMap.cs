namespace Snippets4.Container
{
    using NServiceBus;
    using StructureMap;

    public class Containers_StructureMap
    {
        public void Simple()
        {
            Configure configure = Configure.With();
            #region StructureMap

            configure.StructureMapBuilder();

            #endregion
        }

        public void Existing()
        {
            Configure configure = Configure.With();
            #region StructureMap_Existing
            Container container = new Container(x => x.For<MyService>().Use(new MyService()));
            configure.StructureMapBuilder(container);
            #endregion
        }

    }
}