using Microsoft.Owin;
using Owin;

[assembly: OwinStartup(typeof(Store.ECommerce.Startup))]

namespace Store.ECommerce
{
    public class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            app.MapSignalR();
        }
    }
}
