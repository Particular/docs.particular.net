using Microsoft.Extensions.DependencyInjection;

namespace ClaimCheck_2.ClaimCheck.Custom;

using NServiceBus.ClaimCheck;

#region CustomDataBusDefinition
class CustomClaimCheckDefinition : ClaimCheckDefinition
{
    protected override void ConfigureServices(IServiceCollection services)
    {
        services.AddSingleton<IClaimCheck, CustomClaimCheck>();
    }
}
#endregion
