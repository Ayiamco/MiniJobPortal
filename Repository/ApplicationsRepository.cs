using AutoMapper;
using inSpark.Dtos;
using inSpark.Entities;
using inSpark.Infrastructure.Services;
using inSpark.Interfaces;
using inSpark.Models;
using inSpark.Models.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace inSpark.Repository
{
    public class ApplicationsRepository : IApplicationsRepository
    {
        private readonly ApplicationDbContext _context;

        public ApplicationsRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public void CreateItem(Application application)
        {
            _context.Applications.Add(application);
            _context.SaveChanges();
        }

        public Application ReadItem(Guid id)
        {
            return _context.Applications.Find(id);
        }

        public void UpdateItem(Application application)
        {
            Application applicationInDb = _context.Applications
                .Single(x => x.JobId == application.JobId && x.UserId == application.UserId);
            applicationInDb.ApplicationStatus = application.ApplicationStatus;
            _context.SaveChanges();
        }


        public void DeleteItem(Guid id) { }

        public JobApplicationStatus SaveJobApplication(string userId, Job job)
        {
            var applicant = _context.Applications.Where(x => x.UserId == userId && x.JobId == job.Id);
            if (applicant.Count() != 0) return JobApplicationStatus.AlreadyApplied;

            try
            {
                var timeSpan = job.DeadLine.AddDays(1) - DateTime.Now;
                if (timeSpan < new TimeSpan()) return JobApplicationStatus.ApplicationClosed;
                else if (timeSpan > new TimeSpan())
                {
                    Application application = new Application()
                    {
                        JobId = job.Id,
                        UserId = userId,
                        ApplicationStatus = JobApplicationResponse.NotReviewed,
                    };

                    _context.Applications.Add(application);
                    _context.SaveChanges();
                    return JobApplicationStatus.Successuful;
                }
                return JobApplicationStatus.ApplicationError;
            }

            catch { return JobApplicationStatus.ApplicationError; }
        }

        public IEnumerable<Application> GetApplications(Guid jobId)
        {
            if (jobId == new Guid()) return _context.Applications;

            return _context.Applications.Include("User").Where(x => x.JobId == jobId);
        }

        public ApplicationUser GetApplicantDetails(string applicantId)
        {

            ApplicationUser user = _context.Users.SingleOrDefault(x => x.Id == applicantId);
            if (user == null)
            {
                user = _context.Users.SingleOrDefault(x => x.Email == applicantId);
                if (user == null) throw new Exception("User does not exist");

                return user;
            }
            return user;
        }

        public void UpdateApplicantDetails(UpdateApplicantDetailsViewModel applicant)
        {
            ApplicationUser applicantDetailsInDb = _context.Users.Single(x => x.Id == applicant.Id);
            Mapper.Map(applicant, applicantDetailsInDb);
            _context.SaveChanges();
        }

        public IEnumerable<ApplicationUser> GetAllApplicants()
        {
            return _context.Users;
        }


    }
}

