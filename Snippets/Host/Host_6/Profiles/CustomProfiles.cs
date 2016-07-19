using NServiceBus;
using NServiceBus.Hosting.Profiles;
// ReSharper disable RedundantNameQualifier
#region profile_behavior
class LiteEmailBehavior :
    IHandleProfile<NServiceBus.Lite>
{
    public void ProfileActivated(BusConfiguration busConfiguration)
    {
        // set the NullEmailSender in the container
    }
}

class IntegrationEmailBehavior :
    IHandleProfile<NServiceBus.Integration>
{
    public void ProfileActivated(BusConfiguration busConfiguration)
    {
        // set the FileEmailSender in the container
    }
}

class ProductionEmailBehavior :
    IHandleProfile<NServiceBus.Production>
{
    public void ProfileActivated(BusConfiguration busConfiguration)
    {
        // set the SmtpEmailSender in the container
    }
}
#endregion