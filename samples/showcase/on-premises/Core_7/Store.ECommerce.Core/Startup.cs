using System;
using Alexinea.Autofac.Extensions.DependencyInjection;
using Autofac;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using NServiceBus;

namespace Store.ECommerce.Core
{
    public class Startup
    {

        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public IServiceProvider ConfigureServices(IServiceCollection services)
        {
            services.AddMvc();
            services.AddSignalR();
            var builder = new ContainerBuilder();

            builder.Populate(services);
            
            builder.Register(c => endpoint)
                .As<IEndpointInstance>()
                .SingleInstance();

            var container = builder.Build();

            var endpointConfiguration = new EndpointConfiguration("Store.ECommerce");
            endpointConfiguration.PurgeOnStartup(true);
            endpointConfiguration.ApplyCommonConfiguration(transport =>
            {
                var routing = transport.Routing();
                routing.RouteToEndpoint(typeof(Store.Messages.Commands.SubmitOrder).Assembly, "Store.Messages.Commands", "Store.Sales");
            });
            endpointConfiguration.UseContainer<AutofacBuilder>(
                customizations: customizations =>
                {
                    customizations.ExistingLifetimeScope(container);
                });

            endpoint = Endpoint.Start(endpointConfiguration).GetAwaiter().GetResult();

            return new AutofacServiceProvider(container);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IApplicationLifetime applicationLifetime, IHostingEnvironment env)
        {
            applicationLifetime.ApplicationStopping.Register(OnShutdown);

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseStaticFiles();
            app.UseSignalR(routes =>
            {
                routes.MapHub<OrdersHub>("/ordershub");
            });
            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller=Home}/{action=Index}/{id?}");
            });
        }

        private void OnShutdown()
        {
            endpoint?.Stop().GetAwaiter().GetResult();
        }

        IEndpointInstance endpoint;
    }
}
