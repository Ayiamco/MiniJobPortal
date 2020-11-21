using inSpark.Models;
using inSpark.Models.Entities;
using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Threading.Tasks;
using System.Web;

namespace inSpark.Infrastructure.Services
{
    public static class MailService
    {
        public static async Task SendMail(string recieverEmail, string messageBody, string messageSubject)
        {
            try
            {
                SmtpClient smtpClient = new SmtpClient("smtp.gmail.com", 587);
                smtpClient.UseDefaultCredentials = false;
                smtpClient.Credentials = new System.Net.NetworkCredential()
                {
                    UserName = " sender email address ",
                    Password = "sender email password"
                };
                smtpClient.EnableSsl = true;
                smtpClient.DeliveryMethod = SmtpDeliveryMethod.Network;

                MailMessage mailMessage = new MailMessage()
                {
                    To = { recieverEmail },
                    Body = messageBody,
                    From = new MailAddress(" sender email address "),
                    Subject = messageSubject
                };

                await smtpClient.SendMailAsync(mailMessage);
                smtpClient.Dispose();
                return;
            }
            catch (Exception e) { Console.WriteLine(e); }


        }

        public static async  Task NotifyAdminOfApplicantion(string applicantName ,string jobName)
        {
            string messageBody = $"Hi Admin, \n {applicantName} just applied for the {jobName} Job ";
            IdentityMessage mail=new IdentityMessage()
            {
                Body = messageBody,
                Destination = "admin email address",
                Subject = "Job Application"
            };
            EmailService emailService = new EmailService();
            await  emailService.SendAsync(mail);
            //await SendMail("reciever email address",messageBody,"Job Application");
            return;
        }

        public static async Task NotifyApplicant(Job job, AdminApplicationResponse response,ApplicationUser applicant)
        {
            string message="";
            if (response == AdminApplicationResponse.Accepted)
                message = $"{job.MailSalutation} {applicant.FullName}, \n" +
                    $"{job.AcceptanceMailMessage}";
            else if(response == AdminApplicationResponse.Rejected)
                message = $"{job.MailSalutation} {applicant.FullName}, \n" +
                   $"{job.RejectionMailMessage}";

            IdentityMessage mail = new IdentityMessage()
            {
                Body = message,
                Destination = "admin email address",
                Subject = "New Job Application"
            };
            EmailService emailService = new EmailService();
            await emailService.SendAsync(mail);
            //await SendMail(applicant.Email, message, "Application Status");

            return;
        }
    }
}