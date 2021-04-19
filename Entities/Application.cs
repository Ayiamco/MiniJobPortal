using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;
using inSpark.Entities;
using inSpark.Infrastructure.Services;

namespace inSpark.Models.Entities
{
    public class Application
    {
        [Key]
        [Required]
        [Display(Name = "Job ID")]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid Id { get; set; }

        public ApplicationUser User { get; set; }

        [Required]
        [ForeignKey("User")]
        public string UserId{ get; set; }
        

        public Job Job { get; set; }

        [Required]
        [ForeignKey("Job")]
        public Guid JobId { get; set; }

        public string ApplicationStatus { get; set; }
    }
}

