using System.Threading.Tasks;
using NServiceBus.Transport;
using System.Collections.Generic;

#region watch-interface
interface IWatchDispatches
{
    Task Notify(IEnumerable<TransportOperation> operations);
}
#endregion