using System;
using NServiceBus;
using Raven.Client.Documents;

class LegacyFindTypeTagName
{
    void ConfigureConvention(IDocumentStore documentStore)
    {
        #region 5to6-LegacyDocumentIdConventions
        Func<Type, string> defaultConvention = documentStore.Conventions.FindCollectionName;

        documentStore.Conventions.FindCollectionName = type =>
        {
            if (type == typeof(ShippingSagaData))
            {
                return "";
            }

            if (type == typeof(ShippingPolicy))
            {
                return "";
            }

            return defaultConvention(type);
        };
        #endregion
    }

    class ShippingSagaData : ContainSagaData { }
    class ShippingPolicy : ContainSagaData { }
}
