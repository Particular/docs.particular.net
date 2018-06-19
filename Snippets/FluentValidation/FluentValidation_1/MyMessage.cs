using FluentValidation;
using NServiceBus;

#region FluentValidation_message
public class MyMessage :
    IMessage
{
    public string Content { get; set; }
}

public class MyMessageValidator :
    AbstractValidator<MyMessage>
{
    public MyMessageValidator()
    {
        RuleFor(_ => _.Content)
            .NotEmpty();
    }
}
#endregion