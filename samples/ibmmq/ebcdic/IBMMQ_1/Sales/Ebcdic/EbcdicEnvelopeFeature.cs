
using System.Text;
using NServiceBus.Features;

#region EbcdicEnvelopeFeature
sealed class EbcdicEnvelopeFeature : Feature
{
    protected override void Setup(FeatureConfigurationContext context)
    {
        Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);
        context.AddEnvelopeHandler<EbcdicEnvelopeHandler>();
    }
}
#endregion
