using Microsoft.VisualStudio.TestTools.UnitTesting;
using Ploeh.AutoFixture;
using Tuto.TestUtility.AutoFixture;
using Tuto.Web.Config;
using Tuto.Web.Controllers.Manager;
using Tuto.Web.Mappers;

namespace Tuto.Web.UnitTests.Controllers.ManagerTests
{
    [TestClass]
    public abstract class ManagerComponentsBaseTest
    {
        protected WebAppLaunchContext appContext;
        protected HelpRequestMgrController controller;
        protected Fixture fixture;
        protected DataLayer.Models.Users.Manager loggedInManager;


        [TestInitialize]
        public void managerControllerTestInit()
        {
            this.fixture = new Fixture();
            this.fixture.Customizations.Add(new VirtualMembersOmitter());
            AutoMapperConfiguration.Configure();
            
            this.appContext = new TestWebAppLaunchContext();
            TestsUtilities.bypassAppAuthentification(this.appContext, this.appContext.getConfiguration().mainManager);
        }
    }
}