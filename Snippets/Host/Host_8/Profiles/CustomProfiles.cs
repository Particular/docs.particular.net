using NServiceBus;
using NServiceBus.Hosting.Profiles;
// ReSharper disable RedundantNameQualifier
#region profile_behavior
class LiteEmailBehavior :
    IHandleProfile<NServiceBus.Lite>
{
    public void ProfileActivated(EndpointConfiguration endpointConfiguration)
    {
        // set the NullEmailSender in dependency injection
    }
}

class IntegrationEmailBehavior :
    IHandleProfile<NServiceBus.Integration>
{
    public void ProfileActivated(EndpointConfiguration endpointConfiguration)
    {
        // set the FileEmailSender in dependency injection
    }
}

class ProductionEmailBehavior :
    IHandleProfile<NServiceBus.Production>
{
    public void ProfileActivated(EndpointConfiguration endpointConfiguration)
    {
        // set the SmtpEmailSender in dependency injection
    }
}
#endregion