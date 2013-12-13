using Microsoft.VisualStudio.TestTools.UnitTesting;
using Ploeh.AutoFixture;
using Tuto.TestUtility.AutoFixture;
using Tuto.Web.Config;
using Tuto.Web.Controllers.Manager;
using Tuto.Web.Mappers;

namespace Tuto.Web.UnitTests.Controllers.ManagerTests.GroupSessionTests
{
    [TestClass]
    public class GroupSessionManagerControllerBaseTests
    {
        protected WebAppLaunchContext appContext;
        protected GroupSessionMgrController controller;
        protected Fixture fixture;

        [TestInitialize]
        public void groupSessionManagerControllerTestInit()
        {
            this.fixture = new Fixture();
            this.fixture.Customizations.Add(new VirtualMembersOmitter());
            AutoMapperConfiguration.Configure();

            this.appContext = new TestWebAppLaunchContext();

            this.controller = new GroupSessionMgrController(this.appContext);
        }
    }
}
