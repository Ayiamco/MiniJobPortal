using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace inSpark.Models.Entities
{
    public class Job
    {

        [Key]
        [Required]
        [Display(Name = "Job ID")]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid Id { get; set; }

        [Required]
        [StringLength(100)]
        [Display(Name = "Job Title")]
        public string Title { get; set; }

        [Required]
        [Display(Name = "Application DeadLine")]
        public DateTime DeadLine { get; set; }

        [Required]
        [Display(Name ="Date Published")]
        public DateTime DatePublished { get; set; }
        public int NoOfApplicants { get; set; }

        public ICollection<Application> Applications { get; set; }

        [Required]
        public string JobRequirementPath { get; set; }


        [Required]
        [Display(Name ="Mail Salutation (E.g Dear, Hi, Hello)")]
        public string MailSalutation { get; set; }

        [Required]
        [Display(Name ="Rejection Mail Message.")]
        public string RejectionMailMessage { get; set; }

        [Required]
        [Display(Name = "Acceptance Mail Message.")]
        public string AcceptanceMailMessage { get; set; }
    }
}