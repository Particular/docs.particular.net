using NServiceBus;

// ReSharper disable PossibleNullReferenceException
public class InterfaceMessageCreation
{
    public void Simple()
    {
        IBus Bus = null;

        #region InterfaceMessageCreationV4

        var message = Bus.CreateInstance<MyInterfaceMessage>(o =>
        {
            o.OrderNumber = 1234;
        });
        Bus.Publish(message);

        Bus.Publish<MyInterfaceMessage>(o =>
        {
            o.OrderNumber = 1234;
        });

        #endregion
    }

    public interface MyInterfaceMessage
    {
        int OrderNumber { get; set; }
    }
}
