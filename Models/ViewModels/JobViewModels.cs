
using inSpark.Infrastructure.Services;
using inSpark.Models.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace inSpark.Models.ViewModels
{
  

    public class ReApplyViewModel
    {
        public string ApplicantName { get; set; }
        public Guid JobId { get; set; }
        public string JobTitle { get; set; }
        public string ApplicantId { get; set; }
    }


    public class JobFormViewModel
    {

        [Display(Name = "Job ID")]
        public Guid Id { get; set; }

        [Required]
        [StringLength(100)]
        [Display(Name = "Job Title")]
        public string Title { get; set; }

        [Required]
        [MustBeFutureDate]
        [Display(Name = "Application DeadLine (mm/dd/yyyy)")]
        public DateTime DeadLine { get; set; }

        //[Required]
        public HttpPostedFileBase JobRequirement { get; set; }

        [Required]
        [Display(Name = "Mail Salutation (E.g Dear, Hi, Hello)")]
        public string MailSalutation { get; set; }

        [Required]
        [Display(Name = "Rejection Mail Message.")]
        public string RejectionMailMessage { get; set; }

        [Required]
        [Display(Name = "Acceptance Mail Message.")]
        public string AcceptanceMailMessage { get; set; }
    }

    public class JobApplicationStatusViewModel
    {
        public string JobTitle { get; set; }
        public string ApplicantName { get; set; }
        public JobApplicationStatus JobApplicationStatus { get; set; }
            
    }
}