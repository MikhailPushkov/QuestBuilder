using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(QuestBuild_MVC.Startup))]
namespace QuestBuild_MVC
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
