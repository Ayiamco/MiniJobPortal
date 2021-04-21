using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using inSpark.Entities;
using inSpark.Models.Entities;
using PagedList;
using PagedList.Mvc;

namespace inSpark.Dtos
{
    public class GetApplicationsViewModel
    {
        //Guid jobIdint? i, string filterParam,string searchParam
        public int Page { get; set; }
        public Guid JobId { get; set; }
        public string  ApplicantsName { get; set; }
        public string ApplicationStatus { get; set; }
    }
    public class ApplicantsViewModel
    {
        public Job Job { get; set; }
        public int Page  { get; set; }
        public string ApplicantsName { get; set; }
        public string ApplicationStatus { get; set; }

        public IEnumerable<Application> AllApplicants { get; set; }

    }

    public class ApplicantDetailsViewModel
    {
        public ApplicationUser Applicant { get; set; }
        public Guid JobId { get; set; }
    }

    public class AdminDashboardViewModel
    {
        public int NoOfJobs { get; set; }
        public int NoOfApplicants { get; set; }
        public int NoOfApplications { get; set; }

        public string AdminMail { get; set; }

        public int NoOfRejectedApplications { get; set; }
        public int NoOfAcceptedApplications { get; set; }
        public int NoOfNotReviewedApplications { get; set; }
    }
}