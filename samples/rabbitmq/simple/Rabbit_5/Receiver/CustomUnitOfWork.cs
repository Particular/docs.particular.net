using System;
using System.Data;
using System.Reflection;
using System.Threading.Tasks;
using NServiceBus.UnitOfWork;

namespace Receiver
{
    /// <summary>
    /// Custom implementation of Unit of Work for NServiceBus endpoint.
    /// </summary>
    public class CustomUnitOfWork : IManageUnitsOfWork
    {

        /// <summary>
        ///     Initializes a new instance of the <see cref="CustomUnitOfWork"/> class.
        /// </summary>
        /// <param name="transaction">Db transaction</param>
        public CustomUnitOfWork()
        {
        }

        /// <inheritdoc />
        public Task Begin()
        {

            Console.WriteLine("Transaction Started.");
            return Task.CompletedTask;
        }

        /// <inheritdoc />
        public Task End(Exception exception = null)
        {
                return Task.CompletedTask;
        }
    }
}
