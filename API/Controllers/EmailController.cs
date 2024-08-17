using Microsoft.AspNetCore.Mvc;
using SendGrid.Helpers.Mail;
using SendGrid;

namespace API.Controllers
{
    public class EmailController : BaseController
    {
        public EmailController(IConfiguration configuration) : base(configuration)
        {
        }

        [HttpGet]
        public IActionResult SendDateTime()
        {
            return Ok(DateTime.Now);
        }

        [HttpPost]
        public async Task<IActionResult> SendEmail()
        {
            var apiKey = MessagingConfiguration.Messaging.Email;
            var client = new SendGridClient(apiKey);
            var from = new EmailAddress("no-reply@bootcom.co.uk", "Example User");
            var subject = "Sending with SendGrid is Fun";
            var to = new EmailAddress("chris@bootcom.co.uk", "Chris Boot");
            var plainTextContent = "and easy to do anywhere, even with C#";
            var htmlContent = "<strong>and easy to do anywhere, even with C#</strong>";
            var msg = MailHelper.CreateSingleEmail(from, to, subject, plainTextContent, htmlContent);
            var response = await client.SendEmailAsync(msg);

            if (response.IsSuccessStatusCode)
            {
                return NoContent();
            }

            return BadRequest(response);

        }

    }
}
