using NServiceBus;

public class Upgrades7to8
{
#pragma warning disable CS0618
    public void MessageVisibilityTimeout(EndpointConfiguration endpointConfiguration)
    {
        #region 7to8-sqs-message-visibility-timeout
        var routing = endpointConfiguration.UseTransport(new SqsTransport());
        var migrationSettings = routing.EnableMessageDrivenPubSubCompatibilityMode();
        migrationSettings.MessageVisibilityTimeout(timeoutInSeconds: 10);
        #endregion
    }
#pragma warning restore CS0618
}

