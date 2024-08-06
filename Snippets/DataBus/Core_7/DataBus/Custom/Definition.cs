namespace Core7.DataBus.Custom
{
    using System;
    using NServiceBus.DataBus;

    #region CustomDataBusDefinition
    class CustomDatabusDefinition : DataBusDefinition
    {
        protected override Type ProvidedByFeature()
            => typeof(CustomDatabusFeature);
    }
    #endregion
}
