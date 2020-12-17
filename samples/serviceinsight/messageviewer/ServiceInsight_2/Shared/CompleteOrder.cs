using NServiceBus;

public class CompleteOrder :
    IMessage
{
    public string CreditCard { get; set; }
}