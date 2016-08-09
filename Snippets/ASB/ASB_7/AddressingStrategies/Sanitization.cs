using System;
using NServiceBus;
using NServiceBus.Settings;
using NServiceBus.Transport.AzureServiceBus;

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