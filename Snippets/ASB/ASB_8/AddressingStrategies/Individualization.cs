using System;
using NServiceBus.Settings;
using NServiceBus.Transport.AzureServiceBus;

#region custom-individualization-strategy

public class MyIndividualizationStrategy :
    IIndividualizationStrategy
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