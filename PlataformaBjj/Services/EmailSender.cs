using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.Extensions.Options;
using SendGrid;
using SendGrid.Helpers.Mail;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PlataformaBjj.Services
{
    public class EmailSender : IEmailSender, ITemplateSender
    {
        public EmailOptions Options { get; set; }

        public EmailSender(IOptions<EmailOptions> emailOptions)
        {
            Options = emailOptions.Value;
        }

        public Task SendEmailAsync(string email, string subject, string message)
        {
            return Excecute(Options.SendGridKey, subject, message, email);
        }
        public Task SendTemplateAsync(string email, string templateKey)
        {
            return ExcecuteTemplate(Options.SendGridKey, email, templateKey);
        }
        private Task ExcecuteTemplate(string sendGridKey,  string email, string templateKey)
        {
            var client = new SendGridClient(sendGridKey);
          
            var msg = new SendGridMessage()
            {
                From = new EmailAddress("j.rodriguez@gooddata.com.mx", "LegionBjj"),
            };
            msg.SetTemplateId(templateKey);
            msg.SetTemplateData(new
            {
                name = "LegionBJJ",
                url = "https://localhost.com",

            });
            msg.AddTo(new EmailAddress(email));
            try
            {
                return client.SendEmailAsync(msg);
            }
            catch (Exception ex)
            {


            }
            return null;

        }
        private Task Excecute(string sendGridKey, string subject, string message, string email)
        {
            var client = new SendGridClient(sendGridKey);
            //var msg = new SendGridMessage()
            //{
            //    From = new EmailAddress("j.rodriguez@gooddata.com.mx", "LegionBjj"),
            //    Subject = subject,
            //    PlainTextContent = message,
            //    HtmlContent = message
            //};
            var msg = new SendGridMessage()
            {
                From = new EmailAddress("j.rodriguez@gooddata.com.mx", "LegionBjj"),
                //Subject = subject,
                //PlainTextContent = message,
                //HtmlContent = message
            };
            msg.SetTemplateId("d-62a08a86084d4760ba5fdda78f7f474d");
            msg.SetTemplateData(new
            {
                name = "LegionBJJ",
                url = "https://localhost.com",
                subject="TEst12345"

            });
            msg.AddTo(new EmailAddress(email));
            try
            {
                return client.SendEmailAsync(msg);
            }
            catch (Exception ex)
            {

                
            }
            return null;
        }
    }
}
