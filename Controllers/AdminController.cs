using AutoMapper;
using inSpark.Infrastructure;
using inSpark.Infrastructure.Interfaces;
using inSpark.Infrastructure.Services;
using inSpark.Models.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.IO;
using System.Threading.Tasks;
using inSpark.Dtos;
using PagedList.Mvc;
using PagedList;
using inSpark.Interfaces;
using inSpark.Entities;
using System.Diagnostics;

namespace inSpark.Controllers
{
    [Authorize(Roles = (UserRoles.CanAddJobs))]
    public class AdminController : Controller
    {
        private IJobRepository _jobRepo;
        private IApplicationsRepository _applicationsRepo;
        private IFileSaver _fileSaver; 

       
        public AdminController(IJobRepository jobRepo, IApplicationsRepository applicationsRepo,IFileSaver filesaver)
        {
            _jobRepo = jobRepo;
            _applicationsRepo = applicationsRepo;
            _fileSaver = filesaver;
        }

        // GET: /Admin
        public ActionResult Index(int? i)
        {

            if (User.IsInRole(UserRoles.CanAddJobs))
            {
                IEnumerable<Application> applications = _applicationsRepo.GetApplications(new Guid()).ToList();
                AdminDashboardViewModel model = new AdminDashboardViewModel()
                {
                    AdminMail = User.Identity.Name,
                    NoOfAcceptedApplications = applications
                        .Where(x => x.ApplicationStatus == JobApplicationResponse.Accepted).Count(),
                    NoOfApplicants = _applicationsRepo.GetAllApplicants().Count() - 1,
                    NoOfNotReviewedApplications = applications
                        .Where(x => x.ApplicationStatus == JobApplicationResponse.NotReviewed).Count(),
                    NoOfJobs = _jobRepo.GetActiveJobs().Count(),
                    NoOfApplications= applications.Count(),
                    NoOfRejectedApplications=applications
                         .Where(x => x.ApplicationStatus == JobApplicationResponse.Rejected).Count(),
                };

                return View("AdminDashboard",model);
            }
               
            return RedirectToAction("Login", "Account", new { returnUrl="/Admin/Index"});
        }


        //GET: /Admin/CreateJob
        public ActionResult CreateJob(JobFormViewModel model=null)
        {
            if (model == null) return View("JobForm",null);
          
            return View("JobForm",model);
        }


        //POST: /Admin/SaveJob
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult SaveJob(JobFormViewModel model)
        {
            if (!ModelState.IsValid) return RedirectToAction("CreateJob", model);

            if (!string.IsNullOrWhiteSpace(model.Id)) {
                UpdateJob(model);
                return RedirectToAction("GetJobs");
            }; 
            //Save jobRequirement to FileStorage/JobRequirements Folder
            string fileStoragePath = _fileSaver.SaveFile(model.JobRequirement, Request.RequestContext);

            //map the JobFormViewModel to the Job Entity
            Job jobModel= Mapper.Map<JobFormViewModel, Job>(model);
            jobModel.AcceptanceMailMessage = jobModel.AcceptanceMailMessage.Trim();
            jobModel.RejectionMailMessage = jobModel.RejectionMailMessage.Trim();
            jobModel.JobRequirementPath = fileStoragePath;
            jobModel.DatePublished = DateTime.Now;
            _jobRepo.CreateItem(jobModel);
            return RedirectToAction("GetJobs");
        }

        private void UpdateJob (JobFormViewModel model)
        {
            Job job = Mapper.Map<Job>(model);
            job.Id = new Guid(model.Id);
            string fileStoragePath = _fileSaver.SaveFile(model.JobRequirement, Request.RequestContext);
            job.AcceptanceMailMessage = model.AcceptanceMailMessage.Trim();
            job.RejectionMailMessage = job.RejectionMailMessage.Trim();
            job.JobRequirementPath = fileStoragePath;
            _jobRepo.UpdateItem(job);
        }

        public ActionResult Job (Guid jobId)
        {
            Job job = _jobRepo.ReadItem(jobId);
            return View("Job", job);
        }

        //GET: Admin/GetJobs
        public ActionResult GetJobs(int? i)
        {
            var joblist = _jobRepo.GetActiveJobs().ToList().ToPagedList(i ?? 1, 5);
            return View("JobList", joblist);
        }

        //GET: /Admin/GetApplicants?jobId
        public ActionResult GetApplicants(GetApplicationsViewModel model)
        {
            
            var vmodel = new ApplicantsViewModel() { Job = _jobRepo.ReadItemWithNavProps(model.JobId) };
            if ( !string.IsNullOrEmpty(model.ApplicantsName)  || !string.IsNullOrEmpty(model.ApplicationStatus))
            {
                var applicants = vmodel.Job.Applications.Where(x=>  
                    x.User.FullName.ToLower().Contains(model.ApplicantsName) ||
                    x.ApplicationStatus ==model.ApplicationStatus
                    ).ToList();
                vmodel.Job.Applications = applicants;
                vmodel.ApplicationStatus = model.ApplicationStatus;
                vmodel.ApplicantsName = model.ApplicantsName;
            }
            return View("JobApplicantsList",vmodel);
        }

        //GET: /Admin/UpdateJob?id=id
        public ActionResult UpdateJob(Guid id, string property="")
        {
            
            if (property == "DeadLine")
            {
                //instantly End Job Application 
                _jobRepo.EndJobApplication(id);
                return RedirectToAction("GetJobs");
            }

            else
            {
                var job = _jobRepo.ReadItem(id);
                JobFormViewModel model = Mapper.Map<Job, JobFormViewModel>(job);
                model.Id = job.Id.ToString();
                return View ("JobForm",model );
            }
           
        }

        //GET: /Admin/ViewApplicant?applicantId=&jobId=jobId
        public ActionResult ViewApplicant(string applicantId,Guid jobId)
        {

            return View("ApplicantDetails", new ApplicantDetailsViewModel()
            {
                Applicant=_applicationsRepo.GetApplicantDetails(applicantId),
                JobId= jobId
            });
        }

        public async Task<ActionResult> RespondToApplication(string applicantId, Guid jobId, int adminResponse)
        {
            Application jobApplication = new Application() { JobId=jobId, UserId=applicantId};
            Job job = _jobRepo.ReadItem(jobId);
            ApplicationUser applicant = _applicationsRepo.GetApplicantDetails(applicantId);

            switch ( adminResponse)
            {
                case (int)AdminApplicationResponse.Accepted:
                    jobApplication.ApplicationStatus = "Accepted";
                    _applicationsRepo.UpdateItem(jobApplication);
                    await MailService.NotifyApplicant(job, AdminApplicationResponse.Accepted,applicant);
                    break;
                case (int)AdminApplicationResponse.Rejected:
                    jobApplication.ApplicationStatus = "Rejected";
                    _applicationsRepo.UpdateItem(jobApplication);
                    await MailService.NotifyApplicant(job, AdminApplicationResponse.Rejected, applicant);
                    break;
                default:
                    break;
            }
            
            return RedirectToAction("GetApplicants", new { jobId });
        }


    }
}