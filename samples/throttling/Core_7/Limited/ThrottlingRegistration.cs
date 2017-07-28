using NServiceBus.Pipeline;

#region RegisterStep

class ThrottlingRegistration :
    RegisterStep
{
    public ThrottlingRegistration()
        : base("GitHubApiThrottling", typeof(ThrottlingBehavior), "API throttling for GitHub")
    {
        // base.InsertBefore();
    }
}
#endregion