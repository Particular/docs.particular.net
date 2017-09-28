using NServiceBus.Hosting.Profiles;
// ReSharper disable RedundantNameQualifier
#region profile_behavior
class LiteEmailBehavior :
    IHandleProfile<NServiceBus.Lite>
{
    public void ProfileActivated()
    {
        // set the NullEmailSender in dependency injection
    }
}

class IntegrationEmailBehavior :
    IHandleProfile<NServiceBus.Integration>
{
    public void ProfileActivated()
    {
        // set the FileEmailSender in dependency injection
    }
}

class ProductionEmailBehavior :
    IHandleProfile<NServiceBus.Production>
{
    public void ProfileActivated()
    {
        // set the SmtpEmailSender in dependency injection
    }
}
#endregion