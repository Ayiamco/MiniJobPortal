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
        [AllowAnonymous]
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
                    NoOfApplicants = _applicationsRepo.GetAllApplicants().Count(),
                    NoOfNotReviewedApplications = applications
                        .Where(x => x.ApplicationStatus == JobApplicationResponse.NotReviewed).Count(),
                    NoOfJobs = _jobRepo.GetActiveJobs().Count(),
                    NoOfApplications= applications.Count(),
                    NoOfRejectedApplications=applications
                         .Where(x => x.ApplicationStatus == JobApplicationResponse.Rejected).Count(),
                };

                return View("AdminDashboard",model);
            }
               
            return RedirectToAction("Login", "Account");
        }


        //GET: /Admin/CreateJob
        public ActionResult CreateJob(JobFormViewModel model=null)
        {
            if (model.JobRequirement == null)
                return View("JobForm");
            
            return View("JobForm",model);
        }


        //POST: /Admin/SaveJob
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult SaveJob(JobFormViewModel model)
        {
            if (!ModelState.IsValid) return RedirectToAction("CreateJob", model);

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
        public ActionResult GetApplicants(Guid jobId,int? i, string filterParam,string searchParam )
        {
            
            var model = new ApplicantsViewModel() { Job = _jobRepo.ReadItem(jobId) };
            if ( !String.IsNullOrEmpty(searchParam)  &&  String.IsNullOrEmpty(filterParam))
            {
                var applicants = _applicationsRepo.GetApplications(jobId)
                                                       .Where(x=> x.User.FullName.Contains(searchParam))
                                                       .ToList();
                ViewBag.SearchParam = searchParam;
                ViewBag.FilterParam = "Choose Filter";
                model.AllApplicants = applicants;
                model.ApplicantsInPage = model.AllApplicants.ToPagedList(i ?? 1, 5);
            }
            else if (String.IsNullOrEmpty(searchParam) && !String.IsNullOrEmpty(filterParam))
            {
                var applicants = _applicationsRepo.GetApplications(jobId)
                    .Where(x=>x.ApplicationStatus==filterParam)
                    .ToList();
                ViewBag.SearchParam = "";
                ViewBag.FilterParam = filterParam;

                model.AllApplicants = applicants;
                model.ApplicantsInPage = model.AllApplicants.ToPagedList(i ?? 1, 5);
            }
            else if (!String.IsNullOrEmpty(searchParam) && !String.IsNullOrEmpty(filterParam))
            {
                var applicants = _applicationsRepo.GetApplications(jobId)
                   .Where(x => x.ApplicationStatus == filterParam && x.User.FullName.Contains(searchParam))
                   .ToList();
                model.AllApplicants = applicants;
                model.ApplicantsInPage = model.AllApplicants.ToPagedList(i ?? 1, 5);
                ViewBag.SearchParam = searchParam;
                ViewBag.FilterParam = filterParam;
            }
            else
            {
                var applicants_ = _applicationsRepo.GetApplications(jobId).ToList();
                model.AllApplicants = applicants_;
                model.ApplicantsInPage = model.AllApplicants.ToPagedList(i ?? 1, 5);
                ViewBag.SearchParam = "";
                ViewBag.FilterParam = "Choose Filter";

            }

            if (ViewBag.FilterParam == "Choose Filter")
                ViewBag.FilterParamSearch = "";
            else
                ViewBag.FilterParamSearch = ViewBag.FilterParam;


            return View("JobApplicantsList", model);


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
                //Admin wants to update job details
                JobFormViewModel model = Mapper.Map<Job, JobFormViewModel>(_jobRepo.ReadItem(id));
                return RedirectToAction("CreateJob", new { model });
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