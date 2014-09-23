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
        configuration.UseContainer<NServiceBus.Ninject>();

        // Unity
        //configuration.UseContainer<NServiceBus.Unity>());

        // Spring
        configuration.UseContainer<NServiceBus.Spring>();

        // StructureMap
        configuration.UseContainer<NServiceBus.StructureMap>();

        #endregion
    }

}