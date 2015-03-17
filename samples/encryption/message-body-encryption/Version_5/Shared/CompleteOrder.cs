using NServiceBus;
#region Message
public class CompleteOrder : IMessage
{
    public string CreditCard { get; set; }
}
#endregion