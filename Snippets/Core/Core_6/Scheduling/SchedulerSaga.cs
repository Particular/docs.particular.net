namespace Core6.Scheduling
{
    using System;
    using System.Threading.Tasks;
    using NServiceBus;

    #region ScheduleTaskSaga

    class MySaga : Saga<MySagaData>,
       IAmStartedByMessages<StartSaga>, // Saga is started by a message at endpoint startup
       IHandleTimeouts<ExecuteTask> // task that gets executed when the scheduled time is up.
    {
        protected override void ConfigureHowToFindSaga(SagaPropertyMapper<MySagaData> mapper)
        {
            // To ensure that there is only one saga instance per the task name,
            // regardless of if the endpoint is restarted or not.
            mapper.ConfigureMapping<StartSaga>(message => message.TaskName)
                .ToSaga(sagaData => sagaData.TaskName);
        }

        public async Task Handle(StartSaga message, IMessageHandlerContext context)
        {
            Data.TaskName = message.TaskName;
            // Check to avoid that if the saga is already started, don't initiate any more tasks
            // as those timeout messages will arrive when the specified time is up.
            if (!Data.IsTaskAlreadyScheduled)
            {
                // Setup a timeout for the specified interval for the task to be executed.
                await RequestTimeout<ExecuteTask>(context, TimeSpan.FromMinutes(5))
                    .ConfigureAwait(false);
                Data.IsTaskAlreadyScheduled = true;
            }
        }

        public async Task Timeout(ExecuteTask state, IMessageHandlerContext context)
        {
            // Action that gets executed when the specified time is up
            await context.Send(new CallLegacySystem())
                .ConfigureAwait(false);
            // Reschedule the task
            await RequestTimeout<ExecuteTask>(context, TimeSpan.FromMinutes(5))
                .ConfigureAwait(false);
        }

    }

    // Associated saga data
    public class MySagaData : ContainSagaData
    {
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
