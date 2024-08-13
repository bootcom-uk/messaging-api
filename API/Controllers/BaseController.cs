using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [ApiController]
    [Authorize]
    [Route("/[controller]")]
    public class BaseController : ControllerBase
    {

        public BaseController() { 
        
        }

    }
}
