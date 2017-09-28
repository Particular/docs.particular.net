using NServiceBus;
using NServiceBus.Hosting.Profiles;

#region dependent_profile
class MyProfileHandler :
    IHandleProfile<MyProfile>
{
    public void ProfileActivated(EndpointConfiguration endpointConfiguration)
    {
        // set something else in dependency injection
    }
}
#endregion

class MyProfile :
    IProfile
{
}