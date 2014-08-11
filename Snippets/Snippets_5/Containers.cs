using NServiceBus;

public class Containers
{
    public void Simple()
    {
        #region ContainersV5

        // Autofac
        Configure.With(b => b.UseContainer<NServiceBus.Autofac>());

        // Ninject
        Configure.With(b => b.UseContainer<NServiceBus.Ninject>());

        // Unity
        //Configure.With()
        //Configure.With(b => b.UseContainer<NServiceBus.Unity>());

        // Spring
        Configure.With(b => b.UseContainer<NServiceBus.Spring>());

        // StructureMap
        Configure.With(b => b.UseContainer<NServiceBus.StructureMap>());

        #endregion
    }

}