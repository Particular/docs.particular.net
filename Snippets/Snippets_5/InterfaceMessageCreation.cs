using NServiceBus;

// ReSharper disable PossibleNullReferenceException
public class InterfaceMessageCreation
{
    public void Simple()
    {
        IBus Bus = null;
        // start code InterfaceMessageCreationV5
        Bus.Publish<MyInterfaceMessage>(o =>
        {
            o.OrderNumber = 1234;
        });
        // end code InterfaceMessageCreationV5
    }

    public interface MyInterfaceMessage
    {
        int OrderNumber { get; set; }
    }
}
