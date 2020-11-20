using NServiceBus;

#region ShipWithMapleCommand
public class ShipWithMaple : ICommand
{
    public string OrderId { get; set; }
}
#endregion

#region ShipWithAlpineCommand
public class ShipWithAlpine : ICommand
{
    public string OrderId { get; set; }
}
#endregion

#region ShipmentAcceptedByMapleMessage
public class ShipmentAcceptedByMaple : IMessage
{
}
#endregion

#region ShipmentAcceptedByAlpineMessage
public class ShipmentAcceptedByAlpine : IMessage
{
}
#endregion

#region ShipmentFailedEvent
public class ShipmentFailed : IEvent
{
    public string OrderId { get; set; }
}
#endregion