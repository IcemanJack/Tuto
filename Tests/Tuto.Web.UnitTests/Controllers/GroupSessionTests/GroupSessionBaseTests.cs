using Microsoft.VisualStudio.TestTools.UnitTesting;
using Ploeh.AutoFixture;
using Tuto.TestUtility.AutoFixture;
using Tuto.Web.Config;
using Tuto.Web.Controllers;
using Tuto.Web.Mappers;

namespace Tuto.Web.UnitTests.Controllers.GroupSessionTests
{
    [TestClass]
    public class GroupSessionBaseTests
    {
        protected WebAppLaunchContext appContext;
        protected GroupSessionController controller;
        protected Fixture fixture;

        [TestInitialize]
        public void groupSessionBaseTestsInit()
        {
            this.fixture = new Fixture();
            this.fixture.Customizations.Add(new VirtualMembersOmitter());
            AutoMapperConfiguration.Configure();

            this.appContext = new TestWebAppLaunchContext();

            this.controller = new GroupSessionController(this.appContext);
        }
    }
}
