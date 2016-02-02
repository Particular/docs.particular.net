﻿using NServiceBus;
using NServiceBus.Hosting.Profiles;

#region dependent_profile
class MyProfileHandler : IHandleProfile<MyProfile>
{
    public void ProfileActivated(BusConfiguration config)
    {
        // set something else in the container
    }
}
#endregion

internal class MyProfile : IProfile
{
}