using API.Configuration;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [ApiController]
    [Authorize]
    [Route("/[controller]")]
    public class BaseController : ControllerBase
    {

        internal readonly MessagingConfiguration MessagingConfiguration;

        public BaseController(IConfiguration configuration) {
            MessagingConfiguration = configuration.Get<MessagingConfiguration>()!;
        }

    }
}
