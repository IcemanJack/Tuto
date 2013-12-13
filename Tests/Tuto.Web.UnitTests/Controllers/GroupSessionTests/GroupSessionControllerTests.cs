using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MvcContrib.TestHelper;
using NSubstitute;
using Ploeh.AutoFixture;
using Tuto.DataLayer.Models.GroupSessions;
using Tuto.DataLayer.Models.Users;
using Tuto.Web.ViewModels.GroupSession;

namespace Tuto.Web.UnitTests.Controllers.GroupSessionTests
{
    [TestClass]
    public class GroupSessionControllerTests : GroupSessionBaseTests
    {
        [TestInitialize]
        public void groupSessionControllerTestsInit()
        { }

        [TestMethod]
        public void tutor_list_action_should_return_list_view_with_correct_group_sessions()
        {
            const int NUM_OF_GROUP_SESSION_TO_CREATE = 10;

            // Arrange
            var fakedTutor = this.fixture.Create<Tutor>();
            fakedTutor.groupSessions = this.fixture.CreateMany<AssignedGroupSession>(NUM_OF_GROUP_SESSION_TO_CREATE).ToList();
            TestsUtilities.bypassAppAuthentification(this.appContext, fakedTutor);

            // Act
            var returnedResult = this.controller.tutorList() as ViewResult;

            // Assert
            Assert.IsNotNull(returnedResult);
            var returnedViewModel = returnedResult.ViewData.Model as ICollection<GroupSessionListViewModel>;
            Assert.IsNotNull(returnedViewModel);
            Assert.AreEqual(NUM_OF_GROUP_SESSION_TO_CREATE, returnedViewModel.Count);
        }

        [TestMethod]
        public void tutor_refuse_group_session_action_should_update_on_success()
        {
            // Arrange
            var fakedTutor = this.fixture.Create<Tutor>();
            var fakedGroupSession = this.fixture.Create<AssignedGroupSession>();
            TestsUtilities.bypassAppAuthentification(this.appContext, fakedTutor);

            this.appContext.getRepository().getById<AssignedGroupSession>(-1).ReturnsForAnyArgs(fakedGroupSession);

            // Act
            var returnedResult = this.controller.tutorRefuseGroupSession(-1);

            // Assert
            Assert.IsNotNull(returnedResult);
            this.appContext.getRepository().Received().update(Arg.Is<AssignedGroupSession>(x => x.tutor == null));
            returnedResult.AssertViewRendered().ForView("Tutor_List");
        }
    }
}
