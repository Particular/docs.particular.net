namespace Core4.Scheduling
{
    using System;
    using NServiceBus;
    using NServiceBus.Saga;

    #region ScheduleTaskSaga

    class MySaga : Saga<MySagaData>,
        IAmStartedByMessages<StartSaga>, // Saga is started by a message at endpoint startup
        IHandleTimeouts<ExecuteTask> // task that gets executed when the scheduled time is up.
    {
        public override void ConfigureHowToFindSaga()
        {
            // To ensure that there is only one saga instance per the task name,
            // regardless of if the endpoint is restarted or not.
            ConfigureMapping<StartSaga>(message => message.TaskName)
                .ToSaga(sagaData => sagaData.TaskName);
        }

        public void Handle(StartSaga message)
        {
            Data.TaskName = message.TaskName;
            // Check to avoid that if the saga is already started, don't initiate any more tasks
            // as those timeout messages will arrive when the specified time is up.
            if (!Data.IsTaskAlreadyScheduled)
            {
                // Setup a timeout for the specified interval for the task to be executed.
                RequestTimeout<ExecuteTask>(TimeSpan.FromMinutes(5));
                Data.IsTaskAlreadyScheduled = true;
            }
        }

        public void Timeout(ExecuteTask state)
        {
            // Action that gets executed when the specified time is up
            var callLegacySystem = new CallLegacySystem();
            Bus.Send(callLegacySystem);
            // Reschedule the task
            RequestTimeout<ExecuteTask>(TimeSpan.FromMinutes(5));
        }
    }

    // Associated saga data
    public class MySagaData : ContainSagaData
    {
        [Unique]
        public string TaskName { get; set; }
        public bool IsTaskAlreadyScheduled { get; set; }
    }

    // Message that starts the saga
    public class StartSaga : ICommand
    {
        public string TaskName { get; set; }
    }

    // timeout class
    class ExecuteTask
    {
    }

#endregion

}
