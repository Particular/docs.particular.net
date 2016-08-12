﻿namespace Core6.Features
{
    using System;
    using System.Threading;
    using System.Threading.Tasks;
    using NServiceBus;
    using NServiceBus.Features;

    class MyFeatureWithStartupTasks :
        Feature
    {
        #region FeatureStartupTaskRegistration

        protected override void Setup(FeatureConfigurationContext context)
        {
            context.RegisterStartupTask(new MyStartupTask());
            // or
            context.RegisterStartupTask(() => new MyStartupTask());
        }

        #endregion
    }

#region FeatureStartupTaskDefinition
    class MyStartupTask :
        FeatureStartupTask,
        IDisposable
    {
        ManualResetEventSlim resetEvent = new ManualResetEventSlim();

        protected override async Task OnStart(IMessageSession session)
        {
            resetEvent.Set();
        }

        protected override async Task OnStop(IMessageSession session)
        {
            resetEvent.Reset();
        }

        public void Dispose()
        {
            resetEvent.Dispose();
        }
    }
#endregion
}
