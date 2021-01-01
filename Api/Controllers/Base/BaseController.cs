using Microsoft.Extensions.DependencyInjection;
using Services;

namespace Api.Controllers.Base
{
    public class BaseController : BaseWithLoggingController
    {
        private IEventService _eventService;

        private IProjectService _projectService;

        protected IEventService EventService =>
            _eventService ??= HttpContext?.RequestServices.GetService<IEventService>();

        protected IProjectService ProjectService =>
            _projectService ??= HttpContext?.RequestServices.GetService<IProjectService>();
    }
}