namespace Snippets6.Features
{
    using System;
    using System.Threading;
    using System.Threading.Tasks;
    using NServiceBus;
    using NServiceBus.Features;

    class MyFeature : Feature
    {
        protected override void Setup(FeatureConfigurationContext context)
        {
            #region FeatureStartupTaskRegistration

            context.RegisterStartupTask(new MyStartupTask());
            // or
            context.RegisterStartupTask(() => new MyStartupTask());

            #endregion
        }
    }

#region FeatureStartupTaskDefinition
    class MyStartupTask : FeatureStartupTask, IDisposable
    {
        ManualResetEventSlim resetEvent = new ManualResetEventSlim();

        protected override async Task OnStart(IBusSession session)
        {
            resetEvent.Set();
        }

        protected override async Task OnStop(IBusSession session)
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
