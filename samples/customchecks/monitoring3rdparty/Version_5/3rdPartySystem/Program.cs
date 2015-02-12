using System;
using System.Web.Http;
using System.Web.Http.SelfHost;

class Program3rdParty
{
    static void Main(string[] args)
    {
        var config = new HttpSelfHostConfiguration("http://localhost:57789");

        config.Routes.MapHttpRoute(
            "API Default", "api/{controller}/{id}",
            new { id = RouteParameter.Optional });

        Action selfHosting = null;
        selfHosting = () =>
        {
            using (HttpSelfHostServer server = new HttpSelfHostServer(config))
            {
                // This requires elevated permissions
                server.OpenAsync().Wait();
                Console.WriteLine("Press Enter to stop the 3rd party system.");
                Console.ReadLine();
                server.CloseAsync().Wait();
            }

            Console.WriteLine("Press Enter to restart the 3rd party system.");
            Console.ReadLine();

            selfHosting();
        };

        selfHosting();
    }
}

public class ThirdPartyController : ApiController
{
    public IHttpActionResult Get()
    {
        return Ok();
    }
}