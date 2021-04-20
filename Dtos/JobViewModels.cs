
using inSpark.Infrastructure.Services;
using inSpark.Models.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace inSpark.Dtos
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
        public string Id { get; set; }

        [Required]
        [StringLength(100)]
        [Display(Name = "Job Title *")]
        public string Title { get; set; }

        [Required (ErrorMessage ="DeadLine is required")]
        [MustBeFutureDate]
        [Display(Name = "DeadLine (mm/dd/yyyy)")]
        public DateTime DeadLine { get; set; }

        //[Required]
        public HttpPostedFileBase JobRequirement { get; set; }

        [Required(ErrorMessage ="Salutation is required")]
        [Display(Name = "Mail Salutation (E.g Dear, Hi, Hello)")]
        public string MailSalutation { get; set; }

        [Required(ErrorMessage ="Rejection mail is required")]
        [Display(Name = "Rejection Mail Message.")]
        public string RejectionMailMessage { get; set; }

        [Required(ErrorMessage ="Acceptance mail is required")]
        [Display(Name = "Acceptance Mail Message.")]
        public string AcceptanceMailMessage { get; set; }
    }

    public class UpdateJobViewModel: JobFormViewModel
    {
        [Required]
        public Guid Id { get; set; }

        [Required]
        [Display(Name = "Application DeadLine (mm/dd/yyyy) *")]
        public new DateTime DeadLine { get; set; }
    }

    public class JobApplicationStatusViewModel
    {
        public string JobTitle { get; set; }
        public string ApplicantName { get; set; }
        public JobApplicationStatus JobApplicationStatus { get; set; }
            
    }
}