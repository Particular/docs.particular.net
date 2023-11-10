using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using NServiceBus;

namespace AsyncPagesMVC.Core
{
    public class Program
    {
        public static void Main()
        {
            #region ApplicationStart
            var builder = WebApplication.CreateBuilder();

            builder.Host.UseNServiceBus(context =>
            {
                var endpointConfiguration = new EndpointConfiguration("Samples.Mvc.WebApplication");
                endpointConfiguration.MakeInstanceUniquelyAddressable("1");
                endpointConfiguration.EnableCallbacks();

                endpointConfiguration.UseTransport(new LearningTransport());
                endpointConfiguration.UseSerialization<SystemJsonSerializer>();

                return endpointConfiguration;
            });
            #endregion

            builder.Services.AddMvc();

            var app = builder.Build();

            app.MapControllerRoute("default", "{controller=Home}/{action=SendLinks}/{id?}");

            app.Run();
        }
    }
}
