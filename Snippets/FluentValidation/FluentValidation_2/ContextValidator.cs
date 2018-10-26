using FluentValidation;
using NServiceBus;

#region FluentValidation_ContextValidator
public class ContextValidator :
    AbstractValidator<MyMessage>
{
    public ContextValidator()
    {
        RuleFor(_ => _.Content)
            .Custom((propertyValue, validationContext) =>
            {
                var messageHeaders = validationContext.Headers();
                var pipelineContextBag = validationContext.ContextBag();
                if (propertyValue == "User" &&
                    !messageHeaders.ContainsKey("Auth"))
                {
                    validationContext.AddFailure("Expected Auth header to exist");
                }
            });
    }
}
#endregion