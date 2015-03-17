namespace Snippets_3.Scheduling
{
    using System;
    using NServiceBus;
    using NServiceBus.Saga;

    #region ScheduleTaskSaga

    class MyCustomTaskSaga : Saga<MyCustomTaskSagaData>,
        IAmStartedByMessages<StartMyCustomTaskSaga>, // Saga is started by a message at endpoint startup
        IHandleTimeouts<ExecuteTask> // task that gets executed when the scheduled time is up.
    {
        public override void ConfigureHowToFindSaga()
        {
            // To ensure that there is only one saga instance per the task name, regardless of if the endpoint is restarted or not.
            ConfigureMapping<StartMyCustomTaskSaga>(s => s.TaskName, m => m.TaskName);
        }

        public void Handle(StartMyCustomTaskSaga message)
        {
            Data.TaskName = message.TaskName;
            // Check to avoid that if the saga is already started, we don't initiate any more tasks 
            // as those timeout messages will arrive when the specified time is up.
            if (!Data.IsTaskAlreadyScheduled)
            {
                // Setup a timeout for the specified interval for the task to be executed.
                RequestUtcTimeout<ExecuteTask>(TimeSpan.FromMinutes(5));
                Data.IsTaskAlreadyScheduled = true;
            }
        }

        public void Timeout(ExecuteTask state)
        {
            // Action that gets executed when the specified time is up
            Bus.Send(new CallLegacySystem());
            // Reschedule the task
            RequestUtcTimeout<ExecuteTask>(TimeSpan.FromMinutes(5));
        }
    }

    // Associated saga data
    public class MyCustomTaskSagaData : IContainSagaData
    {
        [Unique]
        public string TaskName { get; set; }
        public bool IsTaskAlreadyScheduled { get; set; }

        public Guid Id { get; set; }
        public string OriginalMessageId { get; set; }
        public string Originator { get; set; }
    }

    // Message that starts the saga
    public class StartMyCustomTaskSaga : ICommand
    {
        public string TaskName { get; set; }
    }

    // timeout class
    class ExecuteTask
    {
    }

#endregion

}
