using NServiceBus;

public class MyRequestMessage :
    ICommand
{
    public string Info { get; set; }
}