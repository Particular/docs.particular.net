using NServiceBus;
using NServiceBus.Hosting.Profiles;

#region dependent_profile
class MyProfileHandler :
    IHandleProfile<MyProfile>
{
    public void ProfileActivated(EndpointConfiguration endpointConfiguration)
    {
        // set something else in the container
    }
}
#endregion

class MyProfile :
    IProfile
{
}