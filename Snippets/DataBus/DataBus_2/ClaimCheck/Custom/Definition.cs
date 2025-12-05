using NServiceBus.Features;
using NServiceBus.Settings;

namespace ClaimCheck_2.ClaimCheck.Custom;

using System;
using NServiceBus.ClaimCheck;

#region CustomDataBusDefinition

class CustomClaimCheckDefinition : ClaimCheckDefinition
{
    protected override void EnableFeature(SettingsHolder settings) => settings.EnableFeature<CustomClaimCheckFeature>();
}

#endregion