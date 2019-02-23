using Nancy;
using Nancy.TinyIoc;
using NServiceBus;

public class Bootstrapper :
    DefaultNancyBootstrapper
{
    private readonly IMessageSession endpointInstance;

	public Bootstrapper(IMessageSession messageSession)
	{
		this.endpointInstance = messageSession;
	}	
	
	protected override void ConfigureApplicationContainer(TinyIoCContainer container)
    {
        base.ConfigureApplicationContainer(container);

        // Register endpoint instance
        container.Register<IMessageSession>(endpointInstance);
    }
}