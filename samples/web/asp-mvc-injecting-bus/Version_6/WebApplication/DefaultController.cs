using System.Threading.Tasks;
using System.Web.Mvc;
using NServiceBus;

#region Controller
public class DefaultController : Controller
{
    IEndpointInstance endpoint;

    public DefaultController(IEndpointInstance endpoint)
    {
        this.endpoint = endpoint;
    }

    public ActionResult Index()
    {
        return View();
    }

    [AllowAnonymous]
    public async Task<ActionResult> Send()
    {
        await endpoint.Send("Samples.Mvc.Endpoint", new MyMessage());
        return RedirectToAction("Index", "Default");
    }
}
#endregion
