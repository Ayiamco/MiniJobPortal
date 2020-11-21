using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using AutoMapper;
using inSpark.Models.Entities;
using inSpark.Models.ViewModels;
using inSpark.Models;

namespace inSpark.App_Start
{
    public class MappingProfile:Profile
    {
        public MappingProfile()
        {
            Mapper.CreateMap<Job, JobFormViewModel>();
            Mapper.CreateMap<JobFormViewModel, Job>();
            Mapper.CreateMap<RegisterViewModel, ApplicationUser>();
            Mapper.CreateMap<ApplicationUser,RegisterViewModel>();
            Mapper.CreateMap<Application, Application>();
            Mapper.CreateMap<ApplicationUser,UpdateApplicantDetailsViewModel>();
            Mapper.CreateMap<UpdateApplicantDetailsViewModel, RegisterViewModel>();
            Mapper.CreateMap<UpdateApplicantDetailsViewModel, ApplicationUser>();
            Mapper.CreateMap<Job, Job>()
                .ForMember(x => x.NoOfApplicants, opt => opt.Ignore())
                .ForMember (x=> x.DatePublished,opt=>opt.Ignore()); 
        }

    }
       
}