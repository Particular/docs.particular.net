namespace Core8.DataBus.Custom
{
    using System;
    using NServiceBus.DataBus;

    #region CustomDataBusDefinition
    class CustomDatabusDefinition : DataBusDefinition
    {
        protected override Type ProvidedByFeature()
        {
            return typeof(CustomDatabusFeature);
        }
    }
    #endregion
}
