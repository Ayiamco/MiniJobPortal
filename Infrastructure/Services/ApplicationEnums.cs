using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace inSpark.Infrastructure.Services
{
    public enum JobApplicationStatus
    {
        Successuful=1, ApplicationClosed=2, ApplicationError=3,AlreadyApplied=4
    }

    public enum AdminApplicationResponse
    {
        Accepted, Rejected, NotReviewed
    }

    public static class JobApplicationResponse
    {
        public const string Accepted = "Accepted";
        public const string Rejected = "Rejected";
        public const string NotReviewed = "NotReviewed";
    }
}