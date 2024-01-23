using System;
using System.Threading.Tasks;
using NServiceBus.UnitOfWork;

class Uow
{
#pragma warning disable CS0618 // Type or member is obsolete
    #region core-8to9-uow
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