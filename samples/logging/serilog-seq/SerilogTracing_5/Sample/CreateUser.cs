using NServiceBus;

public class CreateUser :
    IMessage
{
    public string UserName { get; set; }
    public string FamilyName { get; set; }
    public string GivenNames{ get; set; }
}