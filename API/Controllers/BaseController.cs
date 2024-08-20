using API.Configuration;
using API.Services;
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

        internal readonly DatabaseService DatabaseService;

        public BaseController(IConfiguration configuration, DatabaseService databaseService) {
            MessagingConfiguration = configuration.Get<MessagingConfiguration>()!;
            DatabaseService = databaseService;
        }

    }
}
