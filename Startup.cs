using inSpark.Infrastructure;
using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(inSpark.Startup))]
namespace inSpark
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
            SeedRoles.EnsureCreated();
        }
    }
}
