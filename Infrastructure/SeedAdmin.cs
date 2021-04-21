using inSpark.Entities;
using inSpark.Infrastructure.Services;
using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using Microsoft.AspNet.Identity;
using System.Web.Configuration;

namespace inSpark.Infrastructure
{
    public class SeedAdmin
    {
        private static ApplicationDbContext Context => new ApplicationDbContext();
        private static ApplicationUserManager UserManager
            => new ApplicationUserManager(new UserStore<ApplicationUser>(Context));

        public static void EnsureCreated()
        {
            if (!Context.Users.Any())
            {
                CreateAdmin();
            }
        }

        private static  void CreateAdmin()
        {
            UserManager.Create(
                new ApplicationUser
                {
                    FullName = "Admin Admin",
                    Email = WebConfigurationManager.AppSettings["adminEmail"],
                    Address = "local host address",
                    UserName ="Admin",
                    DateOfBirth = Convert.ToDateTime("04/04/1990")

                }, WebConfigurationManager.AppSettings["adminPassword"]); ;
            var admin=Context.Users.Where(x => x.Email == WebConfigurationManager.AppSettings["adminEmail"])?.SingleOrDefault();
            UserManager.AddToRole(admin.Id, UserRoles.CanAddJobs);
        }
    }
}