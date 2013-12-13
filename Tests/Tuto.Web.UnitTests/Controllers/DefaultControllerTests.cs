using Microsoft.VisualStudio.TestTools.UnitTesting;
using NSubstitute;
using Ploeh.AutoFixture;
using Tuto.DataLayer.Models.Users;
using Tuto.TestUtility.AutoFixture;
using Tuto.Web.Config;
using Tuto.Web.Controllers;
using Tuto.Web.Mappers;

namespace Tuto.Web.UnitTests.Controllers
{
    [TestClass]
    public class DefaultControllerTests
    {
        protected static Fixture fixture;
        protected static WebAppLaunchContext appContext;

        [TestClass]
        public class DefaultControllerTestClass : DefaultController
        {
            public DefaultControllerTestClass() : base(DefaultControllerTests.appContext)
            {}

            [TestInitialize]
            public void initTest()
            {
                fixture = new Fixture();
                fixture.Customizations.Add(new VirtualMembersOmitter());
                AutoMapperConfiguration.Configure();

                this.appContext = new TestWebAppLaunchContext();
            }

            [TestMethod]
            public void test_not_logged_in_user_on_restricted_page_should_redirect()
            {
                // Arrange
                this.setAccessType(PageAccessType.TYPE_USER);

                // Act & Assert
                Assert.IsFalse(this.isLoggedIn());
                Assert.IsNull(this.getLoggedInUser());
                Assert.IsFalse(this.isUserAllowed());
            }

            [TestMethod]
            public void test_logged_in_user_on_restricted_page_is_allowed()
            {
                // Arrange
                var loggedInUser = fixture.Create<Helped>();
                TestsUtilities.bypassAppAuthentification(this.appContext, loggedInUser);

                this.setAccessType(PageAccessType.TYPE_USER);

                // Act & Assert
                Assert.IsTrue(this.isLoggedIn());
                Assert.IsNotNull(this.getLoggedInUser());
                Assert.IsTrue(this.isUserAllowed());
            }

            [TestMethod]
            public void test_logged_in_helped_user_on_tutor_only_page_is_not_allowed()
            {
                // Arrange
                var loggedInUser = fixture.Create<Helped>(); // HelpedUser
                TestsUtilities.bypassAppAuthentification(this.appContext, loggedInUser);

                this.setAccessType(PageAccessType.TYPE_TUTOR);

                // Act & Assert
                Assert.IsTrue(this.isLoggedIn());
                Assert.IsNotNull(this.getLoggedInUser());
                Assert.IsFalse(this.isUserAllowed());
            }

            [TestMethod]
            public void test_logged_in_tutor_user_on_manager_only_page_is_not_allowed()
            {
                // Arrange
                var loggedInUser = fixture.Create<Tutor>();
                TestsUtilities.bypassAppAuthentification(this.appContext, loggedInUser);

                this.setAccessType(PageAccessType.TYPE_USER);

                // Act & Assert
                Assert.IsTrue(this.isLoggedIn());
                Assert.IsNotNull(this.getLoggedInUser());
                Assert.IsFalse(this.isUserAllowed());
            }

            [TestMethod]
            public void test_get_logged_in_user_should_return_valid_user()
            {
                // Arrange
                var loggedInUser = fixture.Create<Helped>();
                this.appContext.getHttpContext().Session["loggedIn"] = true;
                this.appContext.getHttpContext().Session["mail"] = loggedInUser.mail;
                this.appContext.getHttpContext().Session["password"] = loggedInUser.password;

                this.appContext.getRepository().single<Helped>(null).ReturnsForAnyArgs(loggedInUser);

                // Act & Assert
                Assert.IsTrue(this.isLoggedIn());
                Assert.IsNotNull(this.getLoggedInUser());
                this.appContext.getRepository().ReceivedWithAnyArgs().single<Helped>(null);
                Assert.AreSame(this.getLoggedInUser(), loggedInUser);
            }

            [TestMethod]
            public void disconnect_action_should_disconnect_user()
            {
                // Arrange
                var fakedLoggedInHelped = DefaultControllerTests.fixture.Create<Helped>();
                TestsUtilities.bypassAppAuthentification(this.appContext, fakedLoggedInHelped);
                var beforeSessionCount = this.appContext.getHttpContext().Session.Count;

                // Act
                var returnedAction = this.disconnectLoggedInUser();

                // Assert
                Assert.IsNotNull(returnedAction);
                Assert.AreNotEqual(beforeSessionCount, this.appContext.getHttpContext().Session);
                this.appContext.getHttpContext().Session.ReceivedWithAnyArgs().RemoveAll();
                Assert.AreEqual(0, this.appContext.getHttpContext().Session.Count);
            }
        }
    }
}
