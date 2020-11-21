using inSpark.Infrastructure.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using inSpark.Models.Entities;
using inSpark.Models;
using AutoMapper;

namespace inSpark.Infrastructure.Services
{
    public class ApplicantDbService:IApplicantDbService
    {
        public DbContext dbContext { get; set; }

        public ApplicantDbService()
        {
            dbContext = new DbContext();
        }

        public void CreateItem(Application application)
        {
            dbContext.Db.Applications.Add(application);
            dbContext.Db.SaveChanges();
        }

        public Application ReadItem(Guid id)
        {
            return dbContext.Db.Applications.Single(x => x.Id == id);
        }

        public void UpdateItem(Application application) 
        {
            Application applicationInDb=dbContext.Db.Applications
                .Single(x => x.JobId == application.JobId && x.UserId==application.UserId);
            applicationInDb.ApplicationStatus = application.ApplicationStatus;
            dbContext.Db.SaveChanges();
        }


        public void DeleteItem(Guid id) { }

        public JobApplicationStatus SaveJobApplication(string userId, Job job)
        {

            //####################################################
            //Code to Update Application goes here
            // ################################################
            var applicant = dbContext.Db.Applications.Where(x=> x.UserId==userId && x.JobId==job.Id);
            if(applicant.Count() !=0)
            {
                return JobApplicationStatus.AlreadyApplied;
            }
            try 
            { 
            var timeSpan = job.DeadLine.AddDays(1) - DateTime.Now;
            if (timeSpan < new TimeSpan())
                return JobApplicationStatus.ApplicationClosed;
            else if (timeSpan > new TimeSpan())
            {
                    Application application = new Application()
                    {
                        JobId = job.Id,
                        UserId = userId,
                        ApplicationStatus = JobApplicationResponse.NotReviewed,
                };

                dbContext.Db.Applications.Add(application);
                dbContext.Db.SaveChanges();
                return JobApplicationStatus.Successuful;
            }
            return JobApplicationStatus.ApplicationError;
            }

            catch { return JobApplicationStatus.ApplicationError; }




        }

        public IEnumerable<Application> GetApplications( Guid jobId)
        {
            if (jobId==new Guid())
            {
                return dbContext.Db.Applications;
            }
            return dbContext.Db.Applications.Include("User").Where(x => x.JobId == jobId);
        }

        public ApplicationUser GetApplicantDetails(string applicantId)
        {
        
            ApplicationUser user= dbContext.Db.Users.SingleOrDefault(x => x.Id == applicantId);
            if (user == null)
            {
                user = dbContext.Db.Users.SingleOrDefault(x => x.Email == applicantId);
                if (user == null)
                    throw new Exception("User does not exist");
                
                return user;
            }
            return user;     
        }

        public void UpdateApplicantDetails(UpdateApplicantDetailsViewModel applicant)
        {
            ApplicationUser applicantDetailsInDb=dbContext.Db.Users.Single(x=> x.Id==applicant.Id);
            Mapper.Map(applicant,applicantDetailsInDb);
            dbContext.Db.SaveChanges();
        }

        public IEnumerable<ApplicationUser> GetAllApplicants()
        {
            return dbContext.Db.Users;
        }

       
    }
}