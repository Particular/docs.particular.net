using Microsoft.Owin;
using Owin;
using Store.ECommerce;

[assembly: OwinStartup(typeof(Startup))]

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
