using AutoMapper;
using inSpark.Entities;
using inSpark.Interfaces;
using inSpark.Models.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Data.Entity;
using System.Web;

namespace inSpark.Repository
{
    public class JobRepository : IJobRepository
    {
        private ApplicationDbContext _context;
        public JobRepository(ApplicationDbContext context)
        {
            _context = context;
        }


        public void CreateItem(Job obj)
        {
            _context.Jobs.Add(obj);
            _context.SaveChanges();
        }

        public Job ReadItem(Guid JobId)
        {
            return _context.Jobs.Find(JobId);
        }

        public Job ReadItemWithNavProps(Guid JobId)
        {
            return _context.Jobs.Include(x=> x.Applications.Select(w=> w.User)).Where(x=> x.Id==JobId).SingleOrDefault();
        }

        public void UpdateItem(Job job)
        {
            Job jobInDb = _context.Jobs.Find(job.Id);
            Mapper.Map(job, jobInDb);
            _context.SaveChanges();
        }

        public void DeleteItem(Guid id)
        {

        }

        public IEnumerable<Job> GetActiveJobs()
        {
            return _context.Jobs;
        }

        public void UpdateNoOfApplicantsCount(Guid jobId)
        {
            var jobInDb = _context.Jobs.Single(X => X.Id == jobId);
            jobInDb.NoOfApplicants++;
            _context.SaveChanges();
        }

        public void EndJobApplication(Guid id)
        {
            Job jobInDb = ReadItem(id);
            jobInDb.DeadLine = DateTime.Now.AddDays(-1);
            _context.SaveChanges();
        }

        public string GetUserFullName(string userId)
        {
            var applicant = _context.Users.Single(x => x.Id == userId);
            return applicant.FullName;

        }
    }
}
