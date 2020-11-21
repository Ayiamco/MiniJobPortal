﻿using AutoMapper;
using inSpark.Infrastructure;
using inSpark.Infrastructure.Interfaces;
using inSpark.Infrastructure.Services;
using inSpark.Models.Entities;
using inSpark.Models.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.IO;
using System.Threading.Tasks;
using inSpark.Models;
using PagedList.Mvc;
using PagedList;

namespace inSpark.Controllers
{
    [Authorize(Roles = (UserRoles.CanAddJobs))]
    public class AdminController : Controller
    {
        private IJobDbService _jobDbContext;
        private IApplicantDbService _applicantionDbContext;
        private IFileSaver _fileSaver; 

       
        public AdminController(IJobDbService context, IApplicantDbService applicantContext,IFileSaver filesaver)
        {
            _jobDbContext = context;
            _applicantionDbContext = applicantContext;
            _fileSaver = filesaver;
        }

        // GET: /Admin
        [AllowAnonymous]
        public ActionResult Index(int? i)
        {

            if (User.Identity.IsAuthenticated && User.IsInRole(UserRoles.CanAddJobs))
            {
                IEnumerable<Application> applications = _applicantionDbContext.GetApplications(new Guid()).ToList();
                AdminDashboardViewModel model = new AdminDashboardViewModel()
                {
                    AdminMail = User.Identity.Name,
                    NoOfAcceptedApplications = applications
                        .Where(x => x.ApplicationStatus == JobApplicationResponse.Accepted).Count(),
                    NoOfApplicants = _applicantionDbContext.GetAllApplicants().Count(),
                    NoOfNotReviewedApplications = applications
                        .Where(x => x.ApplicationStatus == JobApplicationResponse.NotReviewed).Count(),
                    NoOfJobs = _jobDbContext.GetActiveJobs().Count(),
                    NoOfApplications= applications.Count(),
                    NoOfRejectedApplications=applications
                         .Where(x => x.ApplicationStatus == JobApplicationResponse.Rejected).Count(),
                };

                return View("AdminDashboard",model);
            }
               
            return RedirectToAction("AdminLogin", "Account");
        }


        //GET: /Admin/CreateJob
        public ActionResult CreateJob(JobFormViewModel model=null,Guid id =new Guid())
        {
            
            if (model == null && id==new Guid())
                return View("JobForm");
            else if(id !=new Guid())
            {
                model= Mapper.Map<Job,JobFormViewModel>(_jobDbContext.ReadItem(id));
                return View("JobForm", model);
            }
            
            return View("JobForm",model);
        }


        //POST: /Admin/SaveJob
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult SaveJob(JobFormViewModel model)
        {
            if (!ModelState.IsValid)
                return RedirectToAction("CreateJob", model);

            //Check If JobDeadline is a Future Date
            if (!HelperMethods.IsValidDeadLine(model.DeadLine))
                throw new Exception("Invalid Job Deadline");

            //Save jobRequirement to FileStorage/JobRequirements Folder
            string fileStoragePath = _fileSaver.SaveFile(model.JobRequirement, Request.RequestContext);

            //map the JobFormViewModel to the Job Entity
            Job jobModel;
            jobModel = Mapper.Map<JobFormViewModel, Job>(model);

            //Add the Missing Properties in Job object
            jobModel.JobRequirementPath = fileStoragePath;
            if (model.Id == new Guid())
            {
                //Save Job Entity to DB
                jobModel.DatePublished = DateTime.Now;
                _jobDbContext.CreateItem(jobModel);
            }
            else
            {
                //update job details
                _jobDbContext.UpdateItem(jobModel);
            }
            
            return RedirectToAction("GetJobs");

        }

        public ActionResult Job (Guid jobId)
        {
            Job job = _jobDbContext.ReadItem(jobId);
            return View("Job", job);
        }

        //GET: Admin/GetJobs
        public ActionResult GetJobs(int? i)
        {
            var joblist = _jobDbContext.GetActiveJobs().ToList().ToPagedList(i ?? 1, 5);
            return View("JobList", joblist);
        }

        //GET: /Admin/GetApplicants?jobId
        public ActionResult GetApplicants(Guid jobId,int? i, string filterParam,string searchParam )
        {
            
            var model = new ApplicantsViewModel() { Job = _jobDbContext.ReadItem(jobId) };
            if ( !String.IsNullOrEmpty(searchParam)  &&  String.IsNullOrEmpty(filterParam))
            {
                var applicants = _applicantionDbContext.GetApplications(jobId)
                                                       .Where(x=> x.User.FullName.Contains(searchParam))
                                                       .ToList();
                ViewBag.SearchParam = searchParam;
                ViewBag.FilterParam = "Choose Filter";
                model.AllApplicants = applicants;
                model.ApplicantsInPage = model.AllApplicants.ToPagedList(i ?? 1, 5);
            }
            else if (String.IsNullOrEmpty(searchParam) && !String.IsNullOrEmpty(filterParam))
            {
                var applicants = _applicantionDbContext.GetApplications(jobId)
                    .Where(x=>x.ApplicationStatus==filterParam)
                    .ToList();
                ViewBag.SearchParam = "";
                ViewBag.FilterParam = filterParam;

                model.AllApplicants = applicants;
                model.ApplicantsInPage = model.AllApplicants.ToPagedList(i ?? 1, 5);
            }
            else if (!String.IsNullOrEmpty(searchParam) && !String.IsNullOrEmpty(filterParam))
            {
                var applicants = _applicantionDbContext.GetApplications(jobId)
                   .Where(x => x.ApplicationStatus == filterParam && x.User.FullName.Contains(searchParam))
                   .ToList();
                model.AllApplicants = applicants;
                model.ApplicantsInPage = model.AllApplicants.ToPagedList(i ?? 1, 5);
                ViewBag.SearchParam = searchParam;
                ViewBag.FilterParam = filterParam;
            }
            else
            {
                var applicants_ = _applicantionDbContext.GetApplications(jobId).ToList();
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
                _jobDbContext.EndJobApplication(id);
                return RedirectToAction("GetJobs");
            }

            else
            {
                //Admin wants to update job details
                return RedirectToAction("CreateJob", new { id });
            }
           
        }

        //GET: /Admin/ViewApplicant?applicantId=&jobId=jobId
        public ActionResult ViewApplicant(string applicantId,Guid jobId)
        {

            return View("ApplicantDetails", new ApplicantDetailsViewModel()
            {
                Applicant=_applicantionDbContext.GetApplicantDetails(applicantId),
                JobId= jobId
            });
        }

        public async Task<ActionResult> RespondToApplication(string applicantId, Guid jobId, int adminResponse)
        {
            Application jobApplication = new Application() { JobId=jobId, UserId=applicantId};
            Job job = _jobDbContext.ReadItem(jobId);
            ApplicationUser applicant = _applicantionDbContext.GetApplicantDetails(applicantId);

            switch ( adminResponse)
            {
                case (int)AdminApplicationResponse.Accepted:
                    jobApplication.ApplicationStatus = "Accepted";
                    _applicantionDbContext.UpdateItem(jobApplication);
                    await MailService.NotifyApplicant(job, AdminApplicationResponse.Accepted,applicant);
                    break;
                case (int)AdminApplicationResponse.Rejected:
                    jobApplication.ApplicationStatus = "Rejected";
                    _applicantionDbContext.UpdateItem(jobApplication);
                    await MailService.NotifyApplicant(job, AdminApplicationResponse.Rejected, applicant);
                    break;
                default:
                    break;
            }
            
            return RedirectToAction("GetApplicants", new { jobId });
        }


    }
}