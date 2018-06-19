using System.Reflection;
using NServiceBus;

public class Usage
{
    Usage(EndpointConfiguration endpointConfiguration)
    {
        #region FluentValidation

        var validationConfig = endpointConfiguration.UseFluentValidation();
        validationConfig.AddValidatorsFromAssemblyContaining<MyMessage>();

        #endregion

        #region FluentValidation_disableincoming

        endpointConfiguration.UseFluentValidation(
            incoming: false);

        #endregion

        #region FluentValidation_disableoutgoing

        endpointConfiguration.UseFluentValidation(
            outgoing: false);

        #endregion

        #region FluentValidation_EndpointLifecycle

        endpointConfiguration.UseFluentValidation(ValidatorLifecycle.Endpoint);

        #endregion

        #region FluentValidation_UnitOfWorkLifecycle

        endpointConfiguration.UseFluentValidation(ValidatorLifecycle.UnitOfWork);

        #endregion
    }

    void AddValidators(EndpointConfiguration endpointConfiguration, Assembly assembly)
    {
        #region FluentValidation_AddValidators

        var validationConfig = endpointConfiguration.UseFluentValidation();
        validationConfig.AddValidatorsFromAssemblyContaining<MyMessage>();
        validationConfig.AddValidatorsFromAssemblyContaining(typeof(SomeOtherMessage));
        validationConfig.AddValidatorsFromAssembly(assembly);

        #endregion
    }
}