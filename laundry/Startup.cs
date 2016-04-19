using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(laundry.Startup))]
namespace laundry
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            //ConfigureAuth(app);
        }
    }
}
