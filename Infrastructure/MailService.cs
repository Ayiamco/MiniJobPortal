using inSpark.Entities;
using inSpark.Models;
using inSpark.Models.Entities;
using Microsoft.AspNet.Identity;
using SendGrid;
using SendGrid.Helpers.Mail;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Threading.Tasks;
using System.Web;
using System.Web.Configuration;

namespace inSpark.Infrastructure.Services
{
    public static class MailService
    {
        public static async Task SendEmailAsync(string destination,  string messageBody, string messageSubject)
        {
            var apiKey = WebConfigurationManager.AppSettings["apiKey"];
            var user = WebConfigurationManager.AppSettings["user"];

            var mailClient = new SendGridClient(apiKey);

            var email = new SendGridMessage()
            {
                From = new EmailAddress("tochukwuchinedu21@gmail.com", user),
                Subject = messageSubject,
                PlainTextContent = messageBody,
                HtmlContent = messageBody
            };

            email.AddTo(new EmailAddress(destination));
            email.SetClickTracking(false, false);
            await mailClient.SendEmailAsync(email);
        }
       

        public static async  Task NotifyAdminOfApplicantion(string applicantName ,string jobName)
        {
            string messageBody = $"Hi Admin, \n {applicantName} just applied for the {jobName} Job ";
            IdentityMessage mail=new IdentityMessage()
            {
                Body = messageBody,
                Destination = "ayiamco@gmail.com",
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
                Destination =applicant.Email,
                Subject = "New Job Application"
            };
            EmailService emailService = new EmailService();
            await emailService.SendAsync(mail);
            //await SendMail(applicant.Email, message, "Application Status");

            return;
        }
    }
}