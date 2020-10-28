using NServiceBus;

namespace Messages
{
    #region ShipWithMapleCommand
    public class ShipWithMaple : ICommand
    {
        public string OrderId { get; set; }
    }
    #endregion
}