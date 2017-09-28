using NServiceBus;
using NServiceBus.Hosting.Profiles;

#region dependent_profile
class MyProfileHandler :
    IHandleProfile<MyProfile>
{
    public void ProfileActivated(BusConfiguration busConfiguration)
    {
        // set something else in dependency injection
    }

    public void ProfileActivated(Configure config)
    {
    }
}
#endregion

class MyProfile :
    IProfile
{
}