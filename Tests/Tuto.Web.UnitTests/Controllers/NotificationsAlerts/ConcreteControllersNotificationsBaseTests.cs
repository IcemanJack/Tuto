using Microsoft.VisualStudio.TestTools.UnitTesting;
using Ploeh.AutoFixture;
using Tuto.TestUtility.AutoFixture;
using Tuto.Web.Config;
using Tuto.Web.Mappers;

namespace Tuto.Web.UnitTests.Controllers.NotificationsAlerts
{
    [TestClass]
    public class ConcreteControllersNotificationsBaseTests
    {
        protected WebAppLaunchContext appContext;
        protected Fixture fixture;

        [TestInitialize]
        public void concreteControllersNotificationBaseTestsInit()
        {
            this.fixture = new Fixture();
            this.fixture.Customizations.Add(new VirtualMembersOmitter());
            AutoMapperConfiguration.Configure();

            this.appContext = new TestWebAppLaunchContext();
        }
    }
}
