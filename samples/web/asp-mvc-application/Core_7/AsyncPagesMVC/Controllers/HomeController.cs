using Microsoft.AspNetCore.Mvc;

public class HomeController :
    Controller
{
    public ActionResult SendLinks()
    {
        return View();
    }
}
