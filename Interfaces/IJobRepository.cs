using inSpark.Models.Entities;
using System;
using System.Collections.Generic;

namespace inSpark.Interfaces
{
    public interface IJobRepository
    {
        void CreateItem(Job obj);
        void DeleteItem(Guid id);
        void EndJobApplication(Guid id);
        IEnumerable<Job> GetActiveJobs();
        string GetUserFullName(string userId);
        Job ReadItem(Guid JobId);
        void UpdateItem(Job job);
        void UpdateNoOfApplicantsCount(Guid jobId);
        Job ReadItemWithNavProps(Guid JobId);
    }
}