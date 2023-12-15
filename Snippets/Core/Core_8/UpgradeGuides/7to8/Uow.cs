using System;
using System.Threading.Tasks;
using NServiceBus.Pipeline;
using NServiceBus.UnitOfWork;

class UowOld
{
#pragma warning disable CS0618 // Type or member is obsolete
    #region core-8to9-uow-old
    class MyUnitOfWork : IManageUnitsOfWork
    {
        public Task Begin()
        {
            // start the custom unit of work

            return Task.CompletedTask;
        }

        public Task End(Exception ex = null)
        {
            // end the custom unit of work

            if (ex != null)
            {
                // handle exception
            }

            return Task.CompletedTask;
        }
    }
    #endregion
#pragma warning restore CS0618 // Type or member is obsolete
}

class UowNew
{
#pragma warning disable IDE0059 // Unnecessary assignment of a value
#pragma warning disable CS0168 // Variable is declared but never used
    #region core-8to9-uow-new
    class MyUnitOfWork : Behavior<IIncomingPhysicalMessageContext>
    {
        public override async Task Invoke(IIncomingPhysicalMessageContext context, Func<Task> next)
        {
            // start the custom unit of work

            try
            {
                await next();
            }
            catch (Exception ex)
            {
                // handle exception
            }

            // end the custom unit of work
        }
    }
    #endregion
#pragma warning restore CS0168 // Variable is declared but never used
#pragma warning restore IDE0059 // Unnecessary assignment of a value
}

