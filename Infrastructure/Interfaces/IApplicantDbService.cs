using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using inSpark.Models.Entities;
using inSpark.Infrastructure.Services;
using inSpark.Models;

namespace inSpark.Infrastructure.Interfaces
{
    public interface IApplicantDbService:IDbService<Application,Guid>
    {
        JobApplicationStatus SaveJobApplication(string userId, Job job);
        IEnumerable<Application> GetApplications(Guid jobId);

        ApplicationUser GetApplicantDetails(string applicantId);

        void UpdateApplicantDetails(UpdateApplicantDetailsViewModel applicant);

        IEnumerable<ApplicationUser> GetAllApplicants();
    }
}
