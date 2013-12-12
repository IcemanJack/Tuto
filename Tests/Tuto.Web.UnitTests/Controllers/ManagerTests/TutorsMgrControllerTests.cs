using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NSubstitute;
using Ploeh.AutoFixture;
using Tuto.DataLayer.Enums;
using Tuto.DataLayer.Models.Users;
using Tuto.Web.Controllers.Manager;
using Tuto.Web.ViewModels;

namespace Tuto.Web.UnitTests.Controllers.ManagerTests
{
    [TestClass]
    public class TutorMgrControllerTests : ManagerComponentsBaseTest
    {
        private TutorsMgrController controller;

        [TestInitialize]
        public void tutorListMgrTestsInit()
        {
            this.controller = new TutorsMgrController(this.appContext);
        }

        [TestMethod]
        public void tutorList_action_should_return_valid_list_entries()
        {
            // Arrange   
            const int tutorCount = 5;
            var tutor = this.fixture.CreateMany<Tutor>(tutorCount);
            this.appContext.getRepository().getAll<Tutor>().Returns(tutor.AsQueryable());
            // Action
            var actionResult = this.controller.list() as ViewResult;
            var expectedViewModel = actionResult.Model as IEnumerable<TutorListViewModel>;
            // Assert
            Assert.AreEqual(tutorCount, expectedViewModel.Count());
        }

        [TestMethod]
        public void confirm_button_should_change_selected_tutor_state_to_confirmed()
        {
            // Arrange
            var tutor = this.fixture.Create<Tutor>();
            this.appContext.getRepository().getById<Tutor>(tutor.id).Returns(tutor);

            var mgr = this.fixture.Create<Manager>();
            TestsUtilities.bypassAppAuthentification(this.appContext, mgr);

            // Act
            var actionResult = this.controller.setStateConfirmed(tutor.id);

            // Assert
            Assert.IsTrue(tutor.tutorState == TutorState.CONFIRMED);
            this.appContext.getRepository().Received().update(Arg.Is<Tutor>(x => x.id == tutor.id));

        }

        [TestMethod]
        public void refuse_button_should_change_selected_tutor_state_to_inactive()
        {
            // Arrange
            var tutor = this.fixture.Create<Tutor>();
            this.appContext.getRepository().getById<Tutor>(tutor.id).Returns(tutor);

            var mgr = this.fixture.Create<Manager>();
            TestsUtilities.bypassAppAuthentification(this.appContext, mgr);
            var message = "test purpose";

            // Act
            var actionResult = this.controller.setStateInactive(tutor.id, message);

            // Assert
            Assert.IsTrue(tutor.tutorState == TutorState.INACTIVE);
            this.appContext.getRepository().Received().update(Arg.Is<Tutor>(x => x.id == tutor.id));
        }

    }
}
