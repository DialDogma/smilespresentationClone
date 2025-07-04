using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using System.Web.Services.Description;
using Google.Apis.Auth.OAuth2.Mvc;
using Google.Apis.Gmail.v1;
using Google.Apis.Gmail.v1.Data;
using Google.Apis.Services;
using MimeKit;
using SmileSIncident.Helper;

namespace SmileSIncident.Controllers
{
    public class GoogleApiController:Controller
    {
        public async Task<ActionResult> IndexAsync(CancellationToken cancellationToken)
        {
            var result = await new AuthorizationCodeMvcApp(this,new AppFlowMetadata()).
               AuthorizeAsync(cancellationToken);

            if(result.Credential != null)
            {
                var service = new GmailService(new BaseClientService.Initializer
                {
                    HttpClientInitializer = result.Credential,
                    ApplicationName = "smilesincident"
                });
                UsersResource.LabelsResource.ListRequest request = service.Users.Labels.List("me");

                IList<Label> labels = request.Execute().Labels;

                ViewBag.labels = labels;

                return View();
            }
            else
            {
                return new RedirectResult(result.RedirectUri);
            }
        }

        public void SendEmail(HttpPostedFileBase file,string to,string from,string subject,string bodyText)
        {
            var MailMessage = new MailMessage();
            MailMessage.From = new MailAddress(from);
            MailMessage.To.Add(to);
            MailMessage.Subject = subject;
            MailMessage.Body = bodyText;
            //MailMessage.Attachments = file;

            var service = new GmailService(new BaseClientService.Initializer());

            var mimeMessage = MimeMessage.CreateFromMailMessage(MailMessage);

            var gmailMessage = new Google.Apis.Gmail.v1.Data.Message
            {
                Raw = Encode(mimeMessage.ToString())
            };

            UsersResource.MessagesResource.SendRequest request = service.Users.Messages.Send(gmailMessage,"me");

            request.Execute();
        }

        public static string Encode(string text)
        {
            byte[] bytes = System.Text.Encoding.UTF8.GetBytes(text);

            return System.Convert.ToBase64String(bytes)
                .Replace('+','-')
                .Replace('/','_')
                .Replace("=","");
        }
    }
}