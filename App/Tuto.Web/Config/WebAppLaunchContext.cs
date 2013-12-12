using System.Linq;
using System.Web;
using Tuto.DataLayer.Models.Users;
using Tuto.DataLayer.Repository;

namespace Tuto.Web.Config
{
    public class WebAppLaunchContext
    {
        protected static WebAppConfiguration currentConfiguration;
        protected HttpContextBase currentHttpContext;
        protected IEntityRepository repository;

        public WebAppLaunchContext(IEntityRepository repo, HttpContextBase httpContext)
        {
            this.repository = repo;
            this.currentHttpContext = httpContext;
            
            // main manager
            WebAppLaunchContext.currentConfiguration = new WebAppConfiguration();
            WebAppConfiguration.defaultHoursPerSession = 2;

            currentConfiguration.mainManager = this.repository.getAll<Manager>().FirstOrDefault(x => x.mail == WebAppConfiguration.MAIN_MANAGER_EMAIL);
        }
        
        // default application launch context (used at app run time)
        public WebAppLaunchContext() : this(new EntityRepository(), new HttpContextWrapper(HttpContext.Current))
        {}

        // used in unit test when no custom http context is needed
        public WebAppLaunchContext(IEntityRepository repo) : this(repo, null)
        {}

        // accessors
        public IEntityRepository getRepository()
        {
            return this.repository;
        }

        public HttpContextBase getHttpContext()
        {
            return this.currentHttpContext;
        }

        public WebAppConfiguration getConfiguration()
        {
            return WebAppLaunchContext.currentConfiguration;
        }
    }
}