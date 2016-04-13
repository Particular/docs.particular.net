using NServiceBus.Hosting.Profiles;
#region profile_behavior [3,4]
class LiteEmailBehavior : IHandleProfile<NServiceBus.Lite>
{
    public void ProfileActivated()
    {
        // set the NullEmailSender in the container
    }
}

class IntegrationEmailBehavior : IHandleProfile<NServiceBus.Integration>
{
    public void ProfileActivated()
    {
        // set the FileEmailSender in the container
    }
}

class ProductionEmailBehavior : IHandleProfile<NServiceBus.Production>
{
    public void ProfileActivated()
    {
        // set the SmtpEmailSender in the container
    }
}
#endregion