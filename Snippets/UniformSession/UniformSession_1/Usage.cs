namespace UniformSession_1
{
    using System;
    using System.Threading.Tasks;
    using NServiceBus;
    using NServiceBus.UniformSession;

    #region uniformsession-usage
    class ComponentReused
    {
        IUniformSession session;

        public ComponentReused(IUniformSession session)
        {
            this.session = session;
        }

        public async Task Do()
        {
            await session.Send(new MyMessage())
                .ConfigureAwait(false);
        }
    }

    [Route("api/[controller]")]
    class MyController : Controller
    {
        ComponentReused component;

        public MyController(ComponentReused component)
        {
            this.component = component;
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] Input input)
        {
            await component.Do()
                .ConfigureAwait(false);

            return Ok();
        }
    }

    class MyHandler : IHandleMessages<MyCommand>
    {
        ComponentReused component;

        public MyHandler(ComponentReused component)
        {
            this.component = component;
        }
        public async Task Handle(MyCommand message, IMessageHandlerContext context)
        {
            await component.Do()
                .ConfigureAwait(false);
        }
    }

    #endregion

    class MyMessage { }
    class MyCommand { }

    class Controller
    {
        public IActionResult Ok()
        {
            return default;
        }
    }
    class HttpPostAttribute : Attribute { }
    class FromBodyAttribute : Attribute { }
    class RouteAttribute : Attribute
    {
        string v;

        public RouteAttribute(string v)
        {
            this.v = v;
        }
    }
    class Input { }
    interface IActionResult { }
}