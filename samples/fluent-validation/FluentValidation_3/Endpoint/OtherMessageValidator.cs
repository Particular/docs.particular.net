using FluentValidation;

public class OtherMessageValidator :
    AbstractValidator<OtherMessage>
{
    public OtherMessageValidator()
    {
        RuleFor(_ => _.Content)
            .NotEmpty();
    }
}