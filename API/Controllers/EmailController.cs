using Microsoft.AspNetCore.Mvc;
using System.Net.Mail;
using System.Net;
using API.Models;
using API.Services;
using MongoDB.Driver;
using MongoDB.Bson;

namespace API.Controllers
{
    public class EmailController : BaseController
    {
        public EmailController(IConfiguration configuration, DatabaseService databaseService) : base(configuration, databaseService)
        {
        }

        [HttpGet]
        public IActionResult SendDateTime()
        {
            return Ok(MessagingConfiguration.Messaging.Email);
        }

        internal string Merge(string unmergedString, Dictionary<string, string> mergeFields)
        {
            var output = unmergedString;

            mergeFields.TryAdd("{{CURRENT_YEAR}}", DateTime.Now.Year.ToString());

            if (mergeFields != null)
            {
                foreach (var field in mergeFields)
                {
                    output = output.Replace(field.Key, field.Value, StringComparison.OrdinalIgnoreCase);
                }
            }

            return output;
        }

        internal async Task SendEmail(EmailInformation emailSettings)
        {
            // Configure client
            using SmtpClient smtpClient = new SmtpClient(MessagingConfiguration.Messaging.Email.Host, MessagingConfiguration.Messaging.Email.Port);
            smtpClient.EnableSsl = MessagingConfiguration.Messaging.Email.UseSSL;
            smtpClient.Credentials = new NetworkCredential(MessagingConfiguration.Messaging.Email.From.Username, MessagingConfiguration.Messaging.Email.From.Password);
            // Configure message
            using MailMessage smtpMessage = new MailMessage();
            // Setup the mail sender 
            smtpMessage.From = new MailAddress(MessagingConfiguration.Messaging.Email.From.EmailAddress, MessagingConfiguration.Messaging.Email.From.DisplayName);
            // Setup the mail recipients 
            foreach (var mailRecipient in emailSettings.PrimaryRecipients)
            {
                smtpMessage.To.Add(new MailAddress(mailRecipient.Key, mailRecipient.Value));
            }

            if (emailSettings.CarbonCopyRecipients is not null)
            {
                foreach (var mailRecipient in emailSettings.CarbonCopyRecipients)
                {
                    smtpMessage.CC.Add(new MailAddress(mailRecipient.Key, mailRecipient.Value));
                }
            }

            if (emailSettings.HiddenRecipients is not null)
            {
                foreach (var mailRecipient in emailSettings.HiddenRecipients)
                {
                    smtpMessage.Bcc.Add(new MailAddress(mailRecipient.Key, mailRecipient.Value));
                }
            }

            // Setup the mail body
            var id = new ObjectId(emailSettings.EmailBodyId);
            var database = DatabaseService.GetMongoDatabase();
            var collection = database.GetCollection<EmailTemplateModel>("EmailTemplates");
            var emailRecord = await collection.Find(record => record.Id == id).FirstOrDefaultAsync();

            smtpMessage.IsBodyHtml = emailRecord.IsHtmlEmail;
            smtpMessage.Body = Merge(emailRecord.EmailBody, emailSettings.Data);

            // Setup the mail subject 
            smtpMessage.Subject = Merge(emailRecord.EmailSubject, emailSettings.Data);

            // Send the email 
            await smtpClient.SendMailAsync(smtpMessage);
        }

        [HttpPost]
        public async Task<IActionResult> SendEmailRequest([FromBody] EmailInformation emailSettings)
        {
            try
            {                
                await SendEmail(emailSettings);
                return NoContent();
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
            

        }

    }
}
