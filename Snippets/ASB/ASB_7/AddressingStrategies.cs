using System;
using System.Collections.Generic;
using NServiceBus;
using NServiceBus.AzureServiceBus;
using NServiceBus.AzureServiceBus.Addressing;
using NServiceBus.Configuration.AdvanceExtensibility;

#region custom-validation-strategy

public class MyValidationStrategy : IValidationStrategy
{
    public bool IsValid(string entityPath, EntityType entityType)
    {
        throw new NotImplementedException();
    }
}

#endregion

#region custom-validation-strategy-extension

public static class AzureServiceBusValidationStrategyExtensions
{
    public static AzureServiceBusValidationSettings MySetting(this AzureServiceBusValidationSettings extensionPoint, string value)
    {
        var settings = extensionPoint.GetSettings();

        settings.Set("SomeWellknownKey", value);

        return extensionPoint;
    }
}

#endregion

#region custom-sanitization-strategy

public class MySanitizationStrategy : ISanitizationStrategy
{
    public string Sanitize(string entityPath, EntityType entityType)
    {
        throw new NotImplementedException();
    }
}

#endregion

#region custom-sanitization-strategy-extension

public static class AzureServiceBusSanitizationStrategyExtensions
{
    public static AzureServiceBusSanitizationSettings MySetting(this AzureServiceBusSanitizationSettings extensionPoint)
    {
        var settings = extensionPoint.GetSettings();

        return extensionPoint;
    }
}

#endregion

#region custom-namespace-partitioning-strategy

public class MyNamespacePartitioningStrategy : INamespacePartitioningStrategy
{
    public IEnumerable<RuntimeNamespaceInfo> GetNamespaces(PartitioningIntent partitioningIntent)
    {
        throw new NotImplementedException();
    }
}

#endregion

#region custom-namespace-partitioning-strategy-extension

public static class AzureServiceBusNamespacePartitioningStrategyExtensions
{
    public static AzureServiceBusNamespacePartitioningSettings MySetting(this AzureServiceBusNamespacePartitioningSettings extensionPoint)
    {
        var settings = extensionPoint.GetSettings();

        return extensionPoint;
    }
}

#endregion

#region custom-individualization-strategy

public class MyIndividualizationStrategy : IIndividualizationStrategy
{
    public string Individualize(string endpointname)
    {
        throw new NotImplementedException();
    }
}

#endregion

#region custom-individualization-strategy-extension

public static class AzureServiceBusIndividualizationStrategyExtensions
{
    public static AzureServiceBusIndividualizationSettings MySetting(this AzureServiceBusIndividualizationSettings extensionPoint)
    {
        var settings = extensionPoint.GetSettings();

        return extensionPoint;
    }
}

#endregion

#region custom-composition-strategy

public class MyCompositionStrategy : ICompositionStrategy
{
    public string GetEntityPath(string entityname, EntityType entityType)
    {
        throw new NotImplementedException();
    }
}

#endregion

#region custom-composition-strategy-extension

public static class AzureServiceBusCompositionStrategyExtensions
{
    public static AzureServiceBusCompositionSettings MySetting(this AzureServiceBusCompositionSettings extensionPoint)
    {
        var settings = extensionPoint.GetSettings();

        return extensionPoint;
    }
}

#endregion