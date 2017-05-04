using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(CouldProjectAzureV2.Startup))]
namespace CouldProjectAzureV2
{
    public partial class Startup {
        public void Configuration(IAppBuilder app) {
            ConfigureAuth(app);
        }
    }
}
