namespace UniformSession_2
{
    using System;
    using System.Threading.Tasks;
    using NServiceBus;
    using NServiceBus.UniformSession;

    #region uniformsession-usage

    class ReusedComponent
    {
        IUniformSession session;

        public ReusedComponent(IUniformSession session)
        {
            this.session = session;
        }

        public Task Do()
        {
            return session.Send(new MyMessage());
        }
    }

    [Route("api/[controller]")]
    class MyController :
        Controller
    {
        ReusedComponent component;

        public MyController(ReusedComponent component)
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

    class MyHandler :
        IHandleMessages<MyCommand>
    {
        ReusedComponent component;

        public MyHandler(ReusedComponent component)
        {
            this.component = component;
        }

        public Task Handle(MyCommand message, IMessageHandlerContext context)
        {
            return component.Do();
        }
    }

    #endregion

    class MyMessage
    {
    }

    class MyCommand
    {
    }

    class Controller
    {
        public IActionResult Ok()
        {
            return default;
        }
    }

    class HttpPostAttribute : Attribute
    {
    }

    class FromBodyAttribute : Attribute
    {
    }

    class RouteAttribute : Attribute
    {
        string v;

        public RouteAttribute(string v)
        {
            this.v = v;
        }
    }

    class Input
    {
    }

    interface IActionResult
    {
    }
}