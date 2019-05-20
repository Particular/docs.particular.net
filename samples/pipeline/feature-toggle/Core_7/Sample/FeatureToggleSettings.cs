using System;
using System.Collections.Generic;
using NServiceBus.Pipeline;

public class FeatureToggleSettings
{
    public void AddToggle(Func<IInvokeHandlerContext, bool> toggle) => Toggles.Add(toggle);

    internal IList<Func<IInvokeHandlerContext, bool>> Toggles { get; } = new List<Func<IInvokeHandlerContext, bool>>();
}