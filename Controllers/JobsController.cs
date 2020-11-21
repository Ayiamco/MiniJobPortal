using inSpark.Infrastructure.Interfaces;
using inSpark.Infrastructure.Services;
using inSpark.Models.Entities;
using inSpark.Models.ViewModels;
using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using inSpark.Models;
using System.Threading.Tasks;
using AutoMapper;

namespace inSpark.Controllers
{
    [Authorize]
    public class JobsController : Controller
    {
        public IJobDbService _jobDbContext;
        public IApplicantDbService _applicationDbContext;
        public JobsController(IJobDbService jobContext,IApplicantDbService applicationContext)
        {
            _jobDbContext = jobContext;
            _applicationDbContext = applicationContext;
        }


        // GET: Jobs
        [AllowAnonymous]
        public ActionResult Index()
        {
            var jobs = _jobDbContext.GetActiveJobs();
         
            return View("JobListReadOnly", jobs);
        }

        public async Task<ActionResult> Apply(Guid jobId)
        {
            if (User.IsInRole(UserRoles.CanApplyForJobs))
            {
                var userId = User.Identity.GetUserId();
                Job job = _jobDbContext.ReadItem(jobId);

                //save job application to applications table
                JobApplicationStatus isApplicationSuccessful = _applicationDbContext.SaveJobApplication(userId, job);
                
                if (isApplicationSuccessful == JobApplicationStatus.Successuful)
                {
                    //update no of applicants in jobs table
                    _jobDbContext.UpdateNoOfApplicantsCount(jobId);

                    //Notify Admin of application
                    
                    ApplicationUser applicant = _applicationDbContext.GetApplicantDetails(userId);
                    await MailService.NotifyAdminOfApplicantion(applicant.FullName, job.Title);
                }
                    
                if (isApplicationSuccessful == JobApplicationStatus.AlreadyApplied)
                {

                    return View("ReApply", new ReApplyViewModel()
                    {
                        ApplicantName = _jobDbContext.GetUserFullName(userId),
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
                    JobTitle = _jobDbContext.ReadItem(jobId).Title,
                    JobApplicationStatus = isApplicationSuccessful,
                };
                return View("ApplicationStatus", viewModel);

            }
            else
                return RedirectToAction("Login", "Account", new { message = "Please Login as Applicant to continue." });
   
            
        }

        public ActionResult UpdateApplication(string applicantId)
        {

            ApplicationUser applicantDetails=_applicationDbContext.GetApplicantDetails(applicantId);
            UpdateApplicantDetailsViewModel model;
            model = Mapper.Map<ApplicationUser, UpdateApplicantDetailsViewModel>(applicantDetails);
            return RedirectToAction("UpdateUser","Account",model);

        }
    }
}