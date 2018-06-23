using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(XML_Upload.Startup))]
namespace XML_Upload
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
