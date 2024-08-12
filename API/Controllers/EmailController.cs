using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    public class EmailController : BaseController
    {

        [HttpGet]
        public IActionResult SendDateTime()
        {
            return Ok(DateTime.Now);
        }

    }
}
