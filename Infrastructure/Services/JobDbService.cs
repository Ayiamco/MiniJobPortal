using inSpark.Infrastructure.Interfaces;
using inSpark.Models.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using AutoMapper;

namespace inSpark.Infrastructure.Services
{
    public class JobDbService:IJobDbService
    {
        public DbContext dbContext { get; set; }
        public JobDbService()
        {
            dbContext = new DbContext();
        }

        
        public void CreateItem(Job obj)
        {
            dbContext.Db.Jobs.Add(obj);
            dbContext.Db.SaveChanges();
            

        }
        public Job ReadItem(Guid JobId)
        {//Reads the Job with JobId from Jobs Table
            return dbContext.Db.Jobs.Single(j => j.Id == JobId);
        }

        public void UpdateItem( Job job)
        {
            Job jobInDb = dbContext.Db.Jobs.Single(x => x.Id == job.Id);
            Mapper.Map(job, jobInDb);
            dbContext.Db.SaveChanges();


        }
        public void DeleteItem(Guid id)
        {

        }

        public IEnumerable<Job> GetActiveJobs()
        {
            return dbContext.Db.Jobs;
        }

        public void UpdateNoOfApplicantsCount(Guid jobId)
        {
            var jobInDb = dbContext.Db.Jobs.Single(X => X.Id == jobId);
            jobInDb.NoOfApplicants++;
            dbContext.Db.SaveChanges();     
        }

        public void EndJobApplication(Guid id)
        {
            Job jobInDb = ReadItem(id);
            jobInDb.DeadLine = DateTime.Now.AddDays(-1);
            dbContext.Db.SaveChanges();
        }

        public string GetUserFullName(string userId)
        {
            var applicant= dbContext.Db.Users.Single(x=> x.Id== userId);
            return applicant.FullName;

        }
    }
}