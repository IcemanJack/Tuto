using Microsoft.VisualStudio.TestTools.UnitTesting;
using Ploeh.AutoFixture;
using Tuto.DataLayer.Models.Users;
using Tuto.TestUtility.AutoFixture;
using Tuto.Web.Config;
using Tuto.Web.Controllers;
using Tuto.Web.Mappers;

namespace Tuto.Web.UnitTests.Controllers.HomeTests
{
    [TestClass]
    public class HomeControllerBaseTests
    {
        protected WebAppLaunchContext appContext;
        protected HomeController controller;
        protected Fixture fixture;
        protected User loggedInUser;

        [TestInitialize]
        public void homeControllerTestInit()
        {
            this.fixture = new Fixture();
            this.fixture.Customizations.Add(new VirtualMembersOmitter());
            AutoMapperConfiguration.Configure();

            this.loggedInUser = this.fixture.Create<Tutor>();
            
            this.appContext = new TestWebAppLaunchContext();

            // bypass the system authentification
            TestsUtilities.bypassAppAuthentification(this.appContext, this.loggedInUser);

            this.controller = new HomeController(this.appContext);
        }
    }
}
