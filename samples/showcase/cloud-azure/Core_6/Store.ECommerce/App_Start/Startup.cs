using Microsoft.Owin;
using Owin;

[assembly: OwinStartup(typeof(Startup))]

public class Startup
{
    public void Configuration(IAppBuilder app)
    {
        app.MapSignalR();
    }
}