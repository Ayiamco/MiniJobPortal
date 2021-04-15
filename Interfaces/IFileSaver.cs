using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using inSpark.Models;
using System.Web;
using System.Web.Routing;

namespace inSpark.Infrastructure.Interfaces
{
    public interface IFileSaver
    {
        string SaveFile(HttpPostedFileBase file, RequestContext context);
        
    }
}
