using NServiceBus;

interface IOrderEvent : IEvent;
class OrderPlaced : IOrderEvent;
class ExpressOrderPlaced : OrderPlaced;
