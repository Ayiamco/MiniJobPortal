using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;

namespace inSpark.Infrastructure
{
    public class StorageDirectories
    {
        public const string FileStorage = "~/FileStorage";
        public const string FileStoragePath = "/FileStorage";
        public const string ApplicantsResumeFiles = "~/FileStorage/ApplicantsResumeFiles";
        public const string JobRequirementFiles = "~/FileStorage/JobRequirementFiles";
        public const string ProfilePictureFiles = "~/FileStorage/ProfilePictureFiles";
        public const string JobRequirementFilesPath = "/FileStorage/JobRequirementFiles/";
        public const string ApplicantsResumePath = "/FileStorage/ApplicantsResumeFiles/";
        public const string ProfilePictureFilesPath = "/FileStorage/ProfilePictureFiles/";

    }


    public class CreateStorgeDirectory
    {
        public  static void EnsureCreated()
        {

            if (!Directory.Exists(HttpContext.Current.Server.MapPath(StorageDirectories.FileStoragePath)))
            {
                throw new Exception("FileStorage Directory has not being created");
            };

            if (!Directory.Exists(HttpContext.Current.Server.MapPath(StorageDirectories.JobRequirementFilesPath)))
            {
                throw new Exception("JobRequirementFiles Directory has not being created");
            };

            if (!Directory.Exists(HttpContext.Current.Server.MapPath(StorageDirectories.ProfilePictureFilesPath)))
            {
                throw new Exception("ProfilePictureFiles Directory has not being created");
            };
            if (!Directory.Exists(HttpContext.Current.Server.MapPath(StorageDirectories.ApplicantsResumePath)))
            {
                throw new Exception("ApplicantsResumeFiles Directory has not being created");
            };
        }
        
    }
}