using System;
using System.Web.Mvc;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MvcContrib.TestHelper;
using NSubstitute;
using Ploeh.AutoFixture;
using Tuto.DataLayer.Models.Users;
using Tuto.DataLayer.ModelUtilities;
using Tuto.TestUtility.AutoFixture;
using Tuto.Web.Config;
using Tuto.Web.Controllers;
using Tuto.Web.Mappers;
using Tuto.Web.Utilities;

namespace Tuto.Web.UnitTests.Controllers.AccountTests
{
    [TestClass]
    public class AccountControllerBaseTests
    {
        protected WebAppLaunchContext appContext;
        protected AccountController controller;
        protected Fixture fixture;
        protected Helped loggedInUser;

        [TestInitialize]
        public void accountControllerTestInit()
        {
            this.fixture = new Fixture();
            this.fixture.Customizations.Add(new VirtualMembersOmitter());
            AutoMapperConfiguration.Configure();

            this.loggedInUser = this.fixture.Create<Helped>();

            this.appContext = new TestWebAppLaunchContext();
            this.appContext.getConfiguration().mailSender = Substitute.For<EmailSender>().getBuilder().loadSmtpConfigurationFromAppSettings().getSender();

            TestsUtilities.bypassAppAuthentification(this.appContext, this.loggedInUser);

            this.loggedInUser.scheduleBlocks = ScheduleUtilities.getScheduleBlocksFromJson(
                TestsConstants.DUMMY_SCHEDULE_8_TO_11, this.appContext.getRepository());

            this.controller = new AccountController(this.appContext);
        }
    }
}
