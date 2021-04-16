using inSpark.Entities;
using inSpark.Infrastructure.Services;
using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using Microsoft.AspNet.Identity;

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
                    FullName = "Joseph Achonu",
                    Email = "josephachonu@gmail.com",
                    Address="10 my house address",
                    UserName="josephachonu@gmail.com",
                    DateOfBirth= Convert.ToDateTime("04/04/1990")

                },"Joseph123@");
            var admin=Context.Users.Where(x => x.Email == "josephachonu@gmail.com")?.SingleOrDefault();
            UserManager.AddToRole(admin.Id, UserRoles.CanAddJobs);
        }
    }
}