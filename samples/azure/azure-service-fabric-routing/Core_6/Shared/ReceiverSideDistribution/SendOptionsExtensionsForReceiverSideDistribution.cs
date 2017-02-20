using NServiceBus;
using NServiceBus.Extensibility;

namespace Shared
{
    public static class SendOptionsExtensionsForReceiverSideDistribution
    {
        public static void DoNotOverrideReplyToAddress(this SendOptions options)
        {
            options.GetExtensions().Set(HardcodeReplyToAddressToLogicalAddress.NoReplyToAddressOverride.Instance);
        }
    }
}