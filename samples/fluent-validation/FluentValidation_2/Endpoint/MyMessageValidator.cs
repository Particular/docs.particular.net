using FluentValidation;

#region Fluent-Validator
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