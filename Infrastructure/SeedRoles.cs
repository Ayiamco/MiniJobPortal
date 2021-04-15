using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Microsoft.Owin.Security;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using inSpark.Entities;
using Microsoft.AspNet.Identity.EntityFramework;
using inSpark.Infrastructure.Services;

namespace inSpark.Infrastructure
{
    public static class SeedRoles
    {
        private static ApplicationDbContext Context => new ApplicationDbContext();
        private static ApplicationRoleManager RoleManager
            => new ApplicationRoleManager(new RoleStore<ApplicationRole>(Context));


        public static void EnsureCreated()
        {
            if (!Context.Roles.Any())
            {
                CreateRole(UserRoles.CanAddJobs);
                CreateRole(UserRoles.CanApplyForJobs);
            }
        }

        private static void CreateRole(string name)
        {
            if (!RoleManager.RoleExists(name)) {
                RoleManager.Create(new ApplicationRole { Name = name, CreatedAt = DateTime.Now, UpDatedAt = DateTime.Now });
            }
        }
    }
}