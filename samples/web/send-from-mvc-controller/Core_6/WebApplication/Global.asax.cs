using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using Autofac;
using Autofac.Integration.Mvc;
using NServiceBus;

public class MvcApplication :
    HttpApplication
{
    IEndpointInstance endpoint;


    protected void Application_Start()
    {
        #region ApplicationStart

        var endpointConfiguration = new EndpointConfiguration("Samples.MvcInjection.WebApplication");

        endpointConfiguration.UsePersistence<LearningPersistence>();
        endpointConfiguration.UseTransport<LearningTransport>();

        endpoint = Endpoint.Start(endpointConfiguration).GetAwaiter().GetResult();

        var mvcContainerBuilder = new ContainerBuilder();
        mvcContainerBuilder.RegisterInstance(endpoint);

        // Register MVC controllers.
        mvcContainerBuilder.RegisterControllers(typeof(MvcApplication).Assembly);

        var mvcContainer = mvcContainerBuilder.Build();

        DependencyResolver.SetResolver(new AutofacDependencyResolver(mvcContainer));

        AreaRegistration.RegisterAllAreas();
        RegisterRoutes(RouteTable.Routes);

        #endregion
    }

    protected void Application_End()
    {
        endpoint?.Stop().GetAwaiter().GetResult();
    }

    void RegisterRoutes(RouteCollection routes)
    {
        routes.MapRoute(
            name: "Default",
            url: "{controller}/{action}/{id}",
            defaults: new
            {
                controller = "Default",
                action = "Index",
                id = UrlParameter.Optional
            }
        );
    }
}
