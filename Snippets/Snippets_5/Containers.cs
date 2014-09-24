using NServiceBus;

public class Containers
{
    public void Simple()
    {
        #region ContainersV5

        var configuration = new BusConfiguration();

        // Autofac
        configuration.UseContainer<AutofacBuilder>();

        // Ninject
        configuration.UseContainer<NinjectBuilder>();

        // Unity
        configuration.UseContainer<UnityBuilder>();

        // Spring
        configuration.UseContainer<SpringBuilder>();

        // StructureMap
        configuration.UseContainer<StructureMapBuilder>();

        #endregion
    }

}