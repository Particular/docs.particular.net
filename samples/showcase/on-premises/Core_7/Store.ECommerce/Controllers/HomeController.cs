using Microsoft.AspNetCore.Mvc;

public class HomeController :
    Controller
{
    public ActionResult Index()
    {
        return View();
    }
}