using System;
using System.Web.Mvc;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MvcContrib.TestHelper;
using Ploeh.AutoFixture;
using Tuto.DataLayer.Models.Users;
using Tuto.DataLayer.ModelUtilities;
using Tuto.TestUtility.AutoFixture;
using Tuto.Web.Config;
using Tuto.Web.Controllers;
using Tuto.Web.Mappers;

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
            TestsUtilities.bypassAppAuthentification(this.appContext, this.loggedInUser);

            this.loggedInUser.scheduleBlocks = ScheduleUtilities.getScheduleBlocksFromJson(
                "[{\"start\":\"08:00 am\",\"end\":\"11:00 am\",\"day\":1}]", this.appContext.getRepository());

            this.controller = new AccountController(this.appContext);
        }
    }
}
