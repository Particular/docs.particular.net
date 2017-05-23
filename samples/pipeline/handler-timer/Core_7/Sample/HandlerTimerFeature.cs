﻿using NServiceBus.Features;
using NServiceBus.Pipeline;

#region pipeline-config

public class HandlerTimerFeature :
    Feature
{
    internal HandlerTimerFeature()
    {
        EnableByDefault();
    }

    protected override void Setup(FeatureConfigurationContext context)
    {
        context.Pipeline.Register<Registration>();
    }

    public class Registration :
        RegisterStep
    {
        public Registration()
            : base(
                stepId: "HandlerTimer",
                behavior: typeof(HandlerTimerBehavior),
                description: "Logs a warning if a handler take more than a specified time")
        {
        }
    }
}

#endregion