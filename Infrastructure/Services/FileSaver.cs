
using inSpark.Infrastructure.Interfaces;
using inSpark.Models.ViewModels;
using inSpark.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.IO;
using System.Web.Routing;


namespace inSpark.Infrastructure.Services
{
    public  class FileSaver:  Controller, IFileSaver
    {
        [Authorize(Roles = (UserRoles.CanAddJobs))]
        public  string SaveFile(HttpPostedFileBase file, RequestContext requestContext)
        {
            //change file name
            string fileName = Path.GetFileNameWithoutExtension(file.FileName);
            string extension = Path.GetExtension(file.FileName);
            fileName = fileName + DateTime.Now.ToString("yymmssfff") + extension;

            //adding the new file name to the  file storage path
            string storagePath = "/FileStorage/JobRequirementFiles/" + fileName;

            Initialize(requestContext);
            //prefixing the  fileName with the storage folder path
            fileName = Path.Combine(Server.MapPath("~/FileStorage/JobRequirementFiles"), fileName);

            //saving the file in the file folder
            file.SaveAs(fileName);

            
            return storagePath;
        }

        protected override void Initialize(RequestContext requestContext)
        {
            base.Initialize(requestContext);

            
        }


    }
}