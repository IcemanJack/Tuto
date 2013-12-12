using Microsoft.VisualStudio.TestTools.UnitTesting;
using Ploeh.AutoFixture;
using Tuto.TestUtility.AutoFixture;
using Tuto.Web.Config;
using Tuto.Web.Controllers;
using Tuto.Web.Mappers;

namespace Tuto.Web.UnitTests.Controllers.HelpRequestTests
{
    [TestClass]
    public class HelpRequestControllerBaseTests
    {
        protected WebAppLaunchContext appContext;
        protected HelpRequestController controller;
        protected Fixture fixture;

        [TestInitialize]
        public void helpRequestControllerTestInit()
        {
            this.fixture = new Fixture();
            this.fixture.Customizations.Add(new VirtualMembersOmitter());
            AutoMapperConfiguration.Configure();

            this.appContext = new TestWebAppLaunchContext();
            
            this.controller = new HelpRequestController(this.appContext);
        }
    }
}
