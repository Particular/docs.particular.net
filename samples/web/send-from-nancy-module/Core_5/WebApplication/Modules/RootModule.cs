using Nancy;

public class RootModule :
    NancyModule
{
    public RootModule() : base()
    {
        this.Get["/"] = r => this.Response.AsRedirect("/sendmessage");
    }
}