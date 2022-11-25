using System;

public interface IActionResult
{
}

class OkObjectResult : IActionResult
{
    public OkObjectResult(string s) => throw new NotImplementedException();
}
class TriggerMessage
{
}

class HttpTrigger : Attribute
{
    public HttpTrigger(AuthorizationLevel authorizationLevel, string a, string b)
    {
    }

    public string Route { get; set; }
}

public class HttpRequest
{
}

enum AuthorizationLevel
{
    Function
}