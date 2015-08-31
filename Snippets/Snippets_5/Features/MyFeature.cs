namespace Snippets5.Features
{
    using System;
    using System.Threading;
    using NServiceBus.Features;

    class MyFeature : Feature
    {

#region FeatureStartupTaskRegistration
        public MyFeature()
        {
            RegisterStartupTask<MyStartupTask>();
        }
#endregion

        protected override void Setup(FeatureConfigurationContext context)
        {
            // configure the feature if required
        }
    }

#region FeatureStartupTaskDefinition
    class MyStartupTask : FeatureStartupTask, IDisposable
    {
        ManualResetEventSlim resetEvent = new ManualResetEventSlim();

        protected override void OnStart()
        {
            resetEvent.Set();
        }

        protected override void OnStop()
        {
            resetEvent.Reset();
            base.OnStop();
        }

        public void Dispose()
        {
            resetEvent.Dispose();
        }
    }
#endregion
}
