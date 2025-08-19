namespace Core9.DataBus.Custom
{
    using System;
    using NServiceBus.DataBus;

#pragma warning disable CS0618 // Type or member is obsolete
#pragma warning disable CS0672 // Member overrides obsolete member
    #region CustomDataBusDefinition
    class CustomDatabusDefinition : DataBusDefinition
    {
        protected override Type ProvidedByFeature()
            => typeof(CustomDatabusFeature);
    }
    #endregion
#pragma warning restore CS0618 // Type or member is obsolete
#pragma warning restore CS0672 // Member overrides obsolete member
}