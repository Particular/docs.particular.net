using NServiceBus;

public class MyService :
    WcfService<MyRequestMessage, MyResponseMessage>
{
}