using System.Threading.Tasks;
using NServiceBus;

namespace Core_6.EmptyHandlerAsync
{
#pragma warning disable 1998
    #region EmptyHandlerAsync
    public class DoSomethingHandler : IHandleMessages<DoSomething>
    {
        public async Task Handle(DoSomething message, IMessageHandlerContext context)
        {
            // Do something with the message here!
        }
    }
    #endregion
#pragma warning restore 1998
}