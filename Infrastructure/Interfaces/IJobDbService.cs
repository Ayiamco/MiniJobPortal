using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using inSpark.Models.Entities;
using inSpark.Infrastructure.Services;

namespace inSpark.Infrastructure.Interfaces
{
    public interface IJobDbService:IDbService<Job,Guid>
    {
        IEnumerable<Job> GetActiveJobs();
        string GetUserFullName(string userId);
        void UpdateNoOfApplicantsCount(Guid jobId);

        void EndJobApplication(Guid id);
    }
}
