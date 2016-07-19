using System;
using NServiceBus.AzureServiceBus;
using NServiceBus.AzureServiceBus.Addressing;
using NServiceBus.Settings;

#region custom-sanitization-strategy

public class MySanitizationStrategy :
    ISanitizationStrategy
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