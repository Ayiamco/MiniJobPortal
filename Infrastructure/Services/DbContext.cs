using inSpark.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace inSpark.Infrastructure.Services
{
    public class DbContext : Controller
    {
        public ApplicationDbContext Db { get; set; }

        public DbContext()
        {
            Db = ApplicationDbContext.Create();
        }

        protected override void Dispose(bool disposing)
        {
            Db.Dispose();
        }
    }
}