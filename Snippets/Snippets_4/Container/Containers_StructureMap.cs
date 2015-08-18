namespace Snippets4.Container
{
    using NServiceBus;
    using StructureMap;

    public class Containers_StructureMap
    {
        public void Simple()
        {
            #region StructureMap

            Configure configure = Configure.With();
            configure.StructureMapBuilder();

            #endregion
        }

        public void Existing()
        {
            #region StructureMap_Existing
            Configure configure = Configure.With();
            Container container = new Container(x => x.For<MyService>().Use(new MyService()));
            configure.StructureMapBuilder(container);
            #endregion
        }

    }
}