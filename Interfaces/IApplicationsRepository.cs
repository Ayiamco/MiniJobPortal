using inSpark.Dtos;
using inSpark.Entities;
using inSpark.Infrastructure.Services;
using inSpark.Models.Entities;
using System;
using System.Collections.Generic;

namespace inSpark.Interfaces
{
    public interface IApplicationsRepository
    {
        void CreateItem(Application application);
        void DeleteItem(Guid id);
        IEnumerable<ApplicationUser> GetAllApplicants();
        ApplicationUser GetApplicantDetails(string applicantId);
        IEnumerable<Application> GetApplications(Guid jobId);
        Application ReadItem(Guid id);
        JobApplicationStatus SaveJobApplication(string userId, Job job);
        void UpdateApplicantDetails(UpdateApplicantDetailsViewModel applicant);
        void UpdateItem(Application application);
    }
}