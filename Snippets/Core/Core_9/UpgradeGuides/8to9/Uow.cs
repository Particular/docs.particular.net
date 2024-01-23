using System;
using System.Threading.Tasks;
using NServiceBus.Pipeline;

class Uow
{
#pragma warning disable IDE0059 // Unnecessary assignment of a value
#pragma warning disable CS0168 // Variable is declared but never used
    #region core-8to9-uow
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