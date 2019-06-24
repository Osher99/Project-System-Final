using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EASendMail;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using ServerSideJWT.Infra;
using ServerSideJWT.Models;

namespace ServerSideJWT.Services
{
        public class EAEmailSender : IEAEmailSender
        {
        private readonly IConfiguration _config;
        private readonly ApplicationSettings _appSettings;
        private const string EMAIL_VERIFIED_URI = "http://localhost:4200/user/login";


        public EAEmailSender(IConfiguration config, IOptions<ApplicationSettings> appSettings)
            {
                _config = config;
                _appSettings = appSettings.Value;
            }

            public bool SendEmail(AppUser user, string confirmationTokenLink)
            {
                SmtpMail oMail = new SmtpMail("TryIt");
                SmtpClient oSmtp = new SmtpClient();

                // Set sender email address, please change it to yours
                oMail.From = _appSettings.Email_Address;
                // Set recipient email address, please change it to yours
                oMail.To = user.Email;

                // Set email subject
                oMail.Subject = "Verify Account email";

                // Set email body
                oMail.HtmlBody = $"<div style='border: 2px solid orange;padding:10px " +
                $";border-radius: 8px;'><div><p>Hello , {user.UserName},</div><div><p>" +
                $"Youre almost done! Please click this link below to activate your The Store account and get started." +
                $"</p><div><a href={confirmationTokenLink}>" +
                $"<button style='background-color:green'>Activate Your Account</button></a></div><div><p>Love,</p></div>" +
                $"<div><p>The Store</p></div></div>";

            // Your SMTP server address
            SmtpServer oServer = new SmtpServer("smtp.gmail.com")
            {
                // User and password for ESMTP authentication, if your server doesn't require
                // User authentication, please remove the following codes.
                User = _appSettings.Email_Address,
                Password = _appSettings.Password_Secret,

                // If your smtp server requires TLS connection, please add this line
                // oServer.ConnectType = SmtpConnectType.ConnectSSLAuto;
                // If your smtp server requires implicit SSL connection on 465 port, please add this line
                Port = 587,
                ConnectType = SmtpConnectType.ConnectSSLAuto
            };

            try
                {
                    //Console.WriteLine("start to send email ...");
                    oSmtp.SendMail(oServer, oMail);
                    //Console.WriteLine("email was sent successfully!");
                }
                catch (Exception ep)
                {
                //Console.WriteLine("failed to send email with the following error:");
                //Console.WriteLine(ep.Message);
                throw ep;

            }

            return true;
            }


        public bool ResetPassword(AppUser user, string confirmationTokenLink)
        {
            SmtpMail oMail = new SmtpMail("TryIt");
            SmtpClient oSmtp = new SmtpClient();

            // Set sender email address, please change it to yours
            oMail.From = _appSettings.Email_Address;
            // Set recipient email address, please change it to yours
            oMail.To = user.Email;

            // Set email subject
            oMail.Subject = "Reset Password";

            // Set email body
            oMail.HtmlBody = $"<div style='border: 2px solid orange;padding:10px " +
            $";border-radius: 8px;'><div><p>Hello , {user.UserName},</div><div><p>" +
            $"Please click this link to generate new password, your new passwrod will be sent to u" +
            $"</p><div><a href={confirmationTokenLink}>" +
            $"<button style='background-color:green'>Reset password</button></a></div><div><p>Love,</p></div>" +
            $"<div><p>If you didn't ask for this mail, ignore it</p></div></div>";

            // Your SMTP server address
            SmtpServer oServer = new SmtpServer("smtp.gmail.com")
            {
                // User and password for ESMTP authentication, if your server doesn't require
                // User authentication, please remove the following codes.
                User = _appSettings.Email_Address,
                Password = _appSettings.Password_Secret,

                // If your smtp server requires TLS connection, please add this line
                // oServer.ConnectType = SmtpConnectType.ConnectSSLAuto;
                // If your smtp server requires implicit SSL connection on 465 port, please add this line
                Port = 587,
                ConnectType = SmtpConnectType.ConnectSSLAuto
            };

            try
            {
                //Console.WriteLine("start to send email ...");
                oSmtp.SendMail(oServer, oMail);
                //Console.WriteLine("email was sent successfully!");
            }
            catch (Exception ep)
            {
                //Console.WriteLine("failed to send email with the following error:");
                //Console.WriteLine(ep.Message);
                throw ep;

            }

            return true;
        }

        public bool GeneratePassword(AppUser user, string newPassword)
        {
            SmtpMail oMail = new SmtpMail("TryIt");
            SmtpClient oSmtp = new SmtpClient();

            // Set sender email address, please change it to yours
            oMail.From = _appSettings.Email_Address;
            // Set recipient email address, please change it to yours
            oMail.To = user.Email;

            // Set email subject
            oMail.Subject = "Reset Password";

            // Set email body
            oMail.HtmlBody = $"<div style='border: 2px solid orange;padding:10px " +
            $";border-radius: 8px;'><div><p>Hello , {user.UserName},</div><div><p>" +
           $"<h1> Your Password changed successfully!!! </h1><hr><h3> " +
                    $"You're new generated password is: {newPassword} </h3><hr><p> " +
                    $"You can change your password after you login at your private panel " +
                    $"</p><a href= { EMAIL_VERIFIED_URI }> Please Login now! </a></div></div>";

            // Your SMTP server address
            SmtpServer oServer = new SmtpServer("smtp.gmail.com")
            {
                // User and password for ESMTP authentication, if your server doesn't require
                // User authentication, please remove the following codes.
                User = _appSettings.Email_Address,
                Password = _appSettings.Password_Secret,

                // If your smtp server requires TLS connection, please add this line
                // oServer.ConnectType = SmtpConnectType.ConnectSSLAuto;
                // If your smtp server requires implicit SSL connection on 465 port, please add this line
                Port = 587,
                ConnectType = SmtpConnectType.ConnectSSLAuto
            };

            try
            {
                //Console.WriteLine("start to send email ...");
                oSmtp.SendMail(oServer, oMail);
                //Console.WriteLine("email was sent successfully!");
            }
            catch (Exception ep)
            {
                //Console.WriteLine("failed to send email with the following error:");
                //Console.WriteLine(ep.Message);
                throw ep;

            }

            return true;
        }
    }
}
    