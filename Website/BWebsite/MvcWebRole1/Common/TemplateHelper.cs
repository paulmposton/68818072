using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.IO;
using System.Net;

using System.Net.Mail;

namespace MvcWebRole1.Common
{
    public class TemplateHelper
    {

        /// <summary>
        /// Retrieves the contents of a file (your email template, for example).
        /// </summary>
        /// <param name="path">The path to the file on a local disk, or a remote URL (starting http:// or https://).</param>
        /// <returns></returns>
        public static string GetEmailTemplate(string path)
        {
            if (path.StartsWith("http://") || path.StartsWith("https://")) return GetEmailTemplate(new Uri(path));
            using (var sr = new StreamReader(path)) return sr.ReadToEnd();
        }

        public static string GetEmailTemplate(Uri remoteAddress)
        {
            var result = String.Empty;
            try
            {
                var request = (HttpWebRequest)WebRequest.Create(remoteAddress);
                var response = (HttpWebResponse)request.GetResponse();
                if (response.StatusCode == HttpStatusCode.OK)
                {
                    using (var sr = new StreamReader(response.GetResponseStream())) result = sr.ReadToEnd();
                }
            }
            catch
            {
                throw; // Technically not pretty; but in practice you would probably want to handle this yourself :)
            }
            return result;
        }

        /// <summary>
        /// Sends an email using the parameters you provide, substituting tokens in a collection with values you supply.
        /// </summary>
        /// <param name="to">The receipient of the message.</param>
        /// <param name="from">The sender of the message.</param>
        /// <param name="cc">An array of CC addresses (optional).</param>
        /// <param name="bcc">An array of BCC addresses (optional).</param>
        /// <param name="subject">The subject of the email (also parsed for tokens).</param>
        /// <param name="replacements">A dictionary of replacements to be made.</param>
        /// <param name="body">The body of the message to be sent.</param>
        /// <param name="isBodyHtml">Indicates whether the content of the template is HTML (true) or plain-text (false) and is used to set the IsBodyHtml property of the MailMessage.</param>

        public static void Send(MailAddress to, MailAddress from, MailAddress bcc, string subject, IEnumerable<KeyValuePair<string, string>> replacements, string body, bool isBodyHtml = true)
        {

            using (MailMessage message = new MailMessage())
            {
                // Assign message recipient
                message.To.Add(to);
                message.From = from;
                message.Bcc.Add(bcc);
                // Set body type
                message.IsBodyHtml = isBodyHtml;

                // Replace tokens in the message body and subject line
                foreach (KeyValuePair<string, string> token in replacements)
                {
                    body = body.Replace(token.Key, token.Value);
                    subject = subject.Replace(token.Key, token.Value);
                }

                // Assign message body and subject
                message.Body = body;
                message.Subject = subject;

                // Send the message
                System.Net.Mail.SmtpClient client = new SmtpClient("mail.authsmtp.com", 2525);
                client.DeliveryMethod = SmtpDeliveryMethod.Network;
                client.EnableSsl = false;

                // TODO: needs to use AuthSMTP
                client.Credentials = new NetworkCredential("ac61276", "bnhvcjb4dbuqyf");
                client.Send(message);
            }
        }
    }
}