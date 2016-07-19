using System;
using NServiceBus.AzureServiceBus;
using NServiceBus.AzureServiceBus.Addressing;
using NServiceBus.Settings;

#region custom-composition-strategy

public class MyCompositionStrategy :
    ICompositionStrategy
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