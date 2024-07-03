using System.Threading.Tasks;
using NServiceBus.Transport;
using System.Collections.Generic;

#region watch-interface
interface IDispatchNotifier
{
    Task Notify(IEnumerable<TransportOperation> operations);
}
#endregion