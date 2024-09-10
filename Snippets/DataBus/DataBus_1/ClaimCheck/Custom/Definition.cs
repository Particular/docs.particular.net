namespace ClaimCheck_1.ClaimCheck.Custom;

using System;
using NServiceBus.ClaimCheck;

#region CustomDataBusDefinition
class CustomClaimCheckDefinition : ClaimCheckDefinition
{
    protected override Type ProvidedByFeature()
        => typeof(CustomClaimCheckFeature);
}
#endregion
