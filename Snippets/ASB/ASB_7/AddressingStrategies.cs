using System;
using System.Collections.Generic;
using NServiceBus;
using NServiceBus.AzureServiceBus;
using NServiceBus.AzureServiceBus.Addressing;
using NServiceBus.Configuration.AdvanceExtensibility;
using NServiceBus.Settings;

#region custom-validation-strategy

public class MyValidationStrategy : IValidationStrategy
{
    ReadOnlySettings settings;

    public MyValidationStrategy(ReadOnlySettings settings)
    {
        this.settings = settings;
    }

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
    ReadOnlySettings settings;

    public MySanitizationStrategy(ReadOnlySettings settings)
    {
        this.settings = settings;
    }

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
    ReadOnlySettings settings;

    public MyNamespacePartitioningStrategy(ReadOnlySettings settings)
    {
        this.settings = settings;
    }

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
    ReadOnlySettings settings;

    public MyIndividualizationStrategy(ReadOnlySettings settings)
    {
        this.settings = settings;
    }

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
    ReadOnlySettings settings;

    public MyCompositionStrategy(ReadOnlySettings settings)
    {
        this.settings = settings;
    }

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
