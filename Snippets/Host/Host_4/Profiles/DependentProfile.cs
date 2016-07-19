using NServiceBus;
using NServiceBus.Hosting.Profiles;

#region dependent_profile
class MyProfileHandler :
    IHandleProfile<MyProfile>,
    IWantTheEndpointConfig
{
    public void ProfileActivated()
    {
        if (Config is AnInterfaceICareAbout)
        {
            // set something in the container
            return;
        }
        // set something else in the container
    }

    public IConfigureThisEndpoint Config { get; set; }
}
#endregion

class AnInterfaceICareAbout
{
}

class MyProfile :
    IProfile
{
}