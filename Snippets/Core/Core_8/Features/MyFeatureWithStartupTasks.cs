namespace Core8.Features
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

        protected override Task OnStart(IMessageSession session, CancellationToken cancellationToken)
        {
            resetEvent.Set();
            return Task.CompletedTask;
        }

        protected override Task OnStop(IMessageSession session, CancellationToken cnCancellationToken)
        {
            resetEvent.Reset();
            return Task.CompletedTask;
        }

        public void Dispose()
        {
            resetEvent.Dispose();
        }
    }
#endregion
}
