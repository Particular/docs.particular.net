using Nancy;

namespace WebApplication.Modules
{
    public class RootModule : NancyModule
    {
        public RootModule() : base()
        {
            this.Get["/"] = r => this.Response.AsRedirect("/sendMessage");
        }
    }
}