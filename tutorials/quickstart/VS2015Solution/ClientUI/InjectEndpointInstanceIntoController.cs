namespace ClientUI
{
    using System;
    using System.Reflection;
    using System.Web.Mvc;
    using System.Web.Routing;
    using NServiceBus;

    public class InjectEndpointInstanceIntoController : DefaultControllerFactory
    {
        IEndpointInstance _endpointInstance;

        public InjectEndpointInstanceIntoController(IEndpointInstance endpointInstance)
        {
            _endpointInstance = endpointInstance;
        }

        public override IController CreateController(RequestContext requestContext, string controllerName)
        {
            var controllerType = GetControllerType(requestContext, controllerName);
            return (IController) Activator.CreateInstance(controllerType, BindingFlags.CreateInstance, null, new object[]
            {
                _endpointInstance
            }, null);
        }
    }
}