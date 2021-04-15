
using inSpark.Infrastructure.Services;
using inSpark.Models.Entities;
using inSpark.Dtos;
using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using inSpark.Interfaces;
using System.Threading.Tasks;
using AutoMapper;
using inSpark.Entities;

namespace inSpark.Controllers
{
    [Authorize]
    public class JobsController : Controller
    {
        public IJobRepository _jobRepo;
        public IApplicationsRepository _applicationsRepo;
        public JobsController(IJobRepository jobRepo,IApplicationsRepository applicationsRepo)
        {
            _jobRepo = jobRepo;
            _applicationsRepo = applicationsRepo;
        }


        // GET: Jobs
        [AllowAnonymous]
        public ActionResult Index()
        {
            var jobs = _jobRepo.GetActiveJobs();
         
            return View("JobListReadOnly", jobs);
        }

        public async Task<ActionResult> Apply(Guid jobId)
        {
            if (User.IsInRole(UserRoles.CanApplyForJobs))
            {
                var userId = User.Identity.GetUserId();
                Job job = _jobRepo.ReadItem(jobId);

                //save job application to applications table
                JobApplicationStatus isApplicationSuccessful = _applicationsRepo.SaveJobApplication(userId, job);
                
                if (isApplicationSuccessful == JobApplicationStatus.Successuful)
                {
                    //update no of applicants in jobs table
                    _jobRepo.UpdateNoOfApplicantsCount(jobId);

                    //Notify Admin of application
                    
                    ApplicationUser applicant = _applicationsRepo.GetApplicantDetails(userId);
                    await MailService.NotifyAdminOfApplicantion(applicant.FullName, job.Title);
                }
                    
                if (isApplicationSuccessful == JobApplicationStatus.AlreadyApplied)
                {

                    return View("ReApply", new ReApplyViewModel()
                    {
                        ApplicantName = _jobRepo.GetUserFullName(userId),
                        JobId = jobId,
                        JobTitle=job.Title,
                        ApplicantId = User.Identity.GetUserId()
                    }
                    );
                }
                //Display Application status View
                var viewModel=new JobApplicationStatusViewModel()
                {
                    ApplicantName = User.Identity.Name,
                    JobTitle = _jobRepo.ReadItem(jobId).Title,
                    JobApplicationStatus = isApplicationSuccessful,
                };
                return View("ApplicationStatus", viewModel);

            }
            else
                return RedirectToAction("Login", "Account", new { message = "Please Login as Applicant to continue." });
   
            
        }

        public ActionResult UpdateApplication(string applicantId)
        {

            ApplicationUser applicantDetails=_applicationsRepo.GetApplicantDetails(applicantId);
            UpdateApplicantDetailsViewModel model;
            model = Mapper.Map<ApplicationUser, UpdateApplicantDetailsViewModel>(applicantDetails);
            return RedirectToAction("UpdateUser","Account",model);

        }
    }
}