using System.Web.Mvc;
using NServiceBus;

#region Controller
public class DefaultController :
    Controller
{
    IBus bus;

    public DefaultController(IBus bus)
    {
        this.bus = bus;
    }

    public ActionResult Index()
    {
        return View();
    }

    [AllowAnonymous]
    public ActionResult Send()
    {
        bus.Send("Samples.Mvc.Endpoint", new MyMessage());
        return RedirectToAction("Index", "Default");
    }
}
#endregion
