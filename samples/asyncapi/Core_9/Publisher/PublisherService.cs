using Microsoft.AspNetCore.Mvc;
using Neuroglia.AsyncApi;
using Neuroglia.AsyncApi.Bindings.Http;
using Neuroglia.AsyncApi.Bindings.Mqtt;
using Neuroglia.AsyncApi.v3;

namespace Publisher;

[Neuroglia.AsyncApi.v3.AsyncApi("Streetlights API", "1.0.0", Description = "The **Smartylighting Streetlights API** allows you to remotely manage the city lights.", LicenseName = "Apache 2.0", LicenseUrl = "https://www.apache.org/licenses/LICENSE-2.0")]
[Server("http", "http://fake-http-server.com", AsyncApiProtocol.Http, PathName = "/{environment}", Description = "A sample **HTTP** server declared using attributes", Bindings = "#/components/serverBindings/http")]
[ServerVariable("http", "environment", Description = "The **environment** to use.", Enum = ["dev", "stg", "prod"])]
[HttpServerBinding("http")]
[Channel("lightingMeasuredMQTT", Address = "streets.{streetName}", Description = "This channel is used to exchange messages about lightning measurements.", Servers = ["#/servers/mosquitto"], Bindings = "#/components/channelBindings/mqtt")]
[MqttChannelBinding("mqtt")]
[ChannelParameter("lightingMeasured", "streetName", Description = "The name of the **street** the lights to get measurements for are located in")]
[ApiController]
[Route("[controller]")]
public class PublisherService : ControllerBase
{
    private readonly ILogger<PublisherService> logger;
    private readonly IMessageSession messageSession;

    public PublisherService(ILogger<PublisherService> logger, IMessageSession messageSession)
    {
        this.logger = logger;
        this.messageSession = messageSession;
    }

    [Operation("publish", V3OperationAction.Send, "#/channels/lightingMeasuredMQTT", Description = "Notifies remote **consumers** about environmental lighting conditions for a particular **streetlight**."), Neuroglia.AsyncApi.v3.Tag(Reference = "#/components/tags/measurement")]
    [HttpGet(Name = "")]
    public async Task<string> Get()
    {
            var now = DateTime.UtcNow.ToString();

            await messageSession.Publish(new SomeEventThatIsBeingPublished { SomeValue = now, SomeOtherValue = now });

            return $"Published event at {now}";
    }
}
