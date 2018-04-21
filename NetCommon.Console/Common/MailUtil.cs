using System;
using System.Collections.Generic;
using System.Net.Mail;
using System.Text;
using System.Text.RegularExpressions;

namespace NetCommon.Console.Common
{
    public class MailUtil
    {
        public const string EmailPattern =
            "^(?:[\\w\\!\\#\\$\\%\\&\\'\\*\\+\\-\\/\\=\\?\\^\\`\\{\\|\\}\\~]+\\.)*[\\w\\!\\#\\$\\%\\&\\'\\*\\+\\-\\/\\=\\?\\^\\`\\{\\|\\}\\~]+@(?:(?:(?:[a-zA-Z0-9](?:[a-zA-Z0-9\\-](?!\\.)){0,61}[a-zA-Z0-9]?\\.)+[a-zA-Z0-9](?:[a-zA-Z0-9\\-](?!$)){0,61}[a-zA-Z0-9]?)|(?:\\[(?:(?:[01]?\\d{1,2}|2[0-4]\\d|25[0-5])\\.){3}(?:[01]?\\d{1,2}|2[0-4]\\d|25[0-5])\\]))$";
        /// <summary>
        /// Verifies that a string is in valid e-mail format
        /// </summary>
        /// <param name="email">Email to verify</param>
        /// <returns>true if the string is a valid e-mail address and false if it's not</returns>
        public bool IsValidEmail(string email)
        {
            if (String.IsNullOrEmpty(email)) return false;

            email = email.Trim();
            var result = Regex.IsMatch(email, EmailPattern, RegexOptions.IgnoreCase);
            return result;
        }
        //发邮件设置
        public void SendMail(string smtpServer, int mailPort, string account, string passwd, string subject, string body, string fromUser, List<string> toUsers)
        {
            var msg = new MailMessage();
            for (int i = 0; i < toUsers.Count; i++)
            {
                msg.To.Add(toUsers[i]);
            }
            msg.From = new MailAddress(account, fromUser, System.Text.Encoding.UTF8);
            msg.Subject = subject;
            msg.SubjectEncoding = System.Text.Encoding.UTF8;
            msg.Body = body;
            msg.BodyEncoding = System.Text.Encoding.UTF8;
            msg.IsBodyHtml = true;
            msg.Priority = MailPriority.Normal;

            using (var client = new SmtpClient(smtpServer, mailPort))
            {
                //如果需要用户名和密码
                client.UseDefaultCredentials = false;
                client.Credentials = new System.Net.NetworkCredential(account, passwd);
                object userState = msg;
                try
                {
                    client.Send(msg);
                }
                catch (SmtpException e)
                {
                }
            }
        }
    }
}
