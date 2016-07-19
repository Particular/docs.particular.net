namespace Core3.Scheduling
{
    using System;
    using NServiceBus;
    using NServiceBus.Saga;
    #region ScheduleTaskSaga

    class MySaga :
        Saga<MySagaData>,
        // Saga is started by a message at endpoint startup
        IAmStartedByMessages<StartSaga>,
        // task that gets executed when the scheduled time is up.
        IHandleTimeouts<ExecuteTask>
    {
        public override void ConfigureHowToFindSaga()
        {
            // To ensure that there is only one saga instance per the task name,
            // regardless of if the endpoint is restarted or not.
            ConfigureMapping<StartSaga>(
                sagaData => sagaData.TaskName,
                message => message.TaskName);
        }

        public void Handle(StartSaga message)
        {
            Data.TaskName = message.TaskName;
            // Check to avoid that if the saga is already started, don't initiate any more tasks
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
            var callLegacySystem = new CallLegacySystem();
            Bus.Send(callLegacySystem);
            // Reschedule the task
            RequestUtcTimeout<ExecuteTask>(TimeSpan.FromMinutes(5));
        }
    }

    // Associated saga data
    public class MySagaData :
        IContainSagaData
    {
        [Unique]
        public string TaskName { get; set; }
        public bool IsTaskAlreadyScheduled { get; set; }

        public Guid Id { get; set; }
        public string OriginalMessageId { get; set; }
        public string Originator { get; set; }
    }

    // Message that starts the saga
    public class StartSaga :
        ICommand
    {
        public string TaskName { get; set; }
    }

    // timeout class
    class ExecuteTask
    {
    }

#endregion

}
