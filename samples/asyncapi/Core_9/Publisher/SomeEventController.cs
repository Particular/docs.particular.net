using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("[controller]")]
public class SomeEventController : ControllerBase
{
    private readonly ILogger<SomeEventController> logger;
    private readonly IMessageSession messageSession;

    public SomeEventController(ILogger<SomeEventController> logger, IMessageSession messageSession)
    {
        this.logger = logger;
        this.messageSession = messageSession;
    }

    [HttpGet(Name = "")]
    public async Task Get()
    {
        var now = DateTime.UtcNow.ToString();
        await messageSession.Publish(new SomeEventThatIsBeingPublished { SomeValue = now, SomeOtherValue = now });
        //await messageSession.Publish(new SomeEvent { SomeValue = now, SomeOtherValue = now });
    }
}