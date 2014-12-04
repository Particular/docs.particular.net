using NServiceBus;

// ReSharper disable PossibleNullReferenceException
public class InterfaceMessageCreation
{
    public void Simple()
    {
        IBus Bus = null;

        #region InterfaceMessageCreation

        Bus.Publish<MyInterfaceMessage>(o =>
        {
            o.OrderNumber = 1234;
        });

        #endregion

        IMessageCreator messageCreator = null;
        #region ReflectionInterfaceMessageCreation
        //This type would be derived from some other runtime information
        var messageType = typeof(MyInterfaceMessage);

        var instance = messageCreator.CreateInstance(messageType);

        //use reflection to set properties on the constructed instance

        Bus.Publish(instance);
        #endregion
    }

    public interface MyInterfaceMessage
    {
        int OrderNumber { get; set; }
    }
}