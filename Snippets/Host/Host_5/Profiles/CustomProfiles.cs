using NServiceBus;
using NServiceBus.Hosting.Profiles;
// ReSharper disable RedundantNameQualifier
#region profile_behavior
class LiteEmailBehavior :
    IHandleProfile<NServiceBus.Lite>
{
    public void ProfileActivated(BusConfiguration busConfiguration)
    {
        // set the NullEmailSender in dependency injection
    }

    public void ProfileActivated(Configure config)
    {
    }
}

class IntegrationEmailBehavior :
    IHandleProfile<NServiceBus.Integration>
{
    public void ProfileActivated(BusConfiguration busConfiguration)
    {
        // set the FileEmailSender in dependency injection
    }

    public void ProfileActivated(Configure config)
    {
    }
}

class ProductionEmailBehavior :
    IHandleProfile<NServiceBus.Production>
{
    public void ProfileActivated(BusConfiguration busConfiguration)
    {
        // set the SmtpEmailSender in dependency injection
    }

    public void ProfileActivated(Configure config)
    {
    }
}
#endregion