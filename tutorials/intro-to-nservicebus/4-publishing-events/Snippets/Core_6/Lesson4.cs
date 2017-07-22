using System.Threading.Tasks;
using NServiceBus;

namespace Core_6
{
    #region Event
    public class SomethingHappened :
        IEvent
    {
        public string SomeProperty { get; set; }
    }
    #endregion

    #region EventHandler
    public class SomethingHappenedHandler :
        IHandleMessages<SomethingHappened>
    {
        public Task Handle(SomethingHappened message, IMessageHandlerContext context)
        {
            // Do something with the event here

            return Task.CompletedTask;
        }
    }
    #endregion

}