using System.Web.Mvc;
using AutoMapper;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MvcContrib.TestHelper;
using NSubstitute;
using Ploeh.AutoFixture;
using Tuto.DataLayer.Models;
using Tuto.DataLayer.Models.Users;
using Tuto.Web.UnitTests.Generic;
using Tuto.Web.ViewModels;

namespace Tuto.Web.UnitTests.Controllers.HelpRequestTests
{
    [TestClass]
    public class HelpRequestControllerCreateTests : HelpRequestControllerBaseTests
    {
        [TestMethod]
        public void create_action_should_return_create_view()
        {
            // Arrange
            var helped = this.fixture.Create<Helped>();
            TestsUtilities.bypassAppAuthentification(this.appContext, helped);

            AssertFunctions.assertValidRenderedViewForName(controller.create(), "Create");
        }

        [TestMethod]
        public void post_create_with_invalid_course_id_should_fail()
        {
            // Arrange
            var helped = this.fixture.Create<Helped>();
            TestsUtilities.bypassAppAuthentification(this.appContext, helped);

            var invalidViewModel = this.fixture.Create<HelpRequestAddViewModel>();
            invalidViewModel.courseId = -1;

            AssertFunctions.assertValidRenderedViewForName(controller.create(invalidViewModel), "Create");

            Assert.IsTrue(this.controller.ModelState.ContainsKey("ErrorInvalidCourseId"), "The modelstate must contain ErrorInvalidCourseId key");
        }

        [TestMethod]
        public void post_create_with_invalid_json_schedule_syntax_should_fail()
        {
            // Arrange
            var helped = this.fixture.Create<Helped>();
            TestsUtilities.bypassAppAuthentification(this.appContext, helped);

            var invalidViewModel = this.fixture.Create<HelpRequestAddViewModel>();
            invalidViewModel.scheduleJson = "[{\"start\":\"08:00 am\",";

            AssertFunctions.assertValidRenderedViewForName(controller.create(invalidViewModel), "Create");

            Assert.IsTrue(this.controller.ModelState.ContainsKey("InvalidScheduleJson"), "The modelstate must contain InvalidSchedule");
        }

        [TestMethod]
        public void post_create_with_invalid_schedule_syntax_should_fail()
        {
            // Arrange
            var helped = this.fixture.Create<Helped>();
            TestsUtilities.bypassAppAuthentification(this.appContext, helped);

            var invalidViewModel = this.fixture.Create<HelpRequestAddViewModel>();
            invalidViewModel.scheduleJson = "[{\"invalidformat\":\"08:00 am\",\"end\":\"02:00 pm\",\"day\":1}]";

            AssertFunctions.assertValidRenderedViewForName(controller.create(invalidViewModel), "Create");
            Assert.IsTrue(this.controller.ModelState.ContainsKey("InvalidScheduleJson"), "The modelstate must contain InvalidSchedule");
        }

        [TestMethod]
        public void post_create_with_valid_inputs_should_succeed()
        {
            // Arrange
            var validViewModel = this.fixture.Create<HelpRequestAddViewModel>();
            validViewModel.scheduleJson = "[{\"start\":\"08:00 am\",\"end\":\"02:00 pm\",\"day\":1}]";

            var expectedHelpedUser = this.fixture.Create<Helped>();
            TestsUtilities.bypassAppAuthentification(this.appContext, expectedHelpedUser);

            var expectedCourse = this.fixture.Create<Course>();

            this.appContext.getRepository().getById<Helped>(expectedHelpedUser.id).ReturnsForAnyArgs(expectedHelpedUser);
            this.appContext.getRepository().getById<Course>(expectedCourse.id).ReturnsForAnyArgs(expectedCourse);

            var expectedCreatedModel = Mapper.Map<HelpRequest>(validViewModel);
            expectedCreatedModel.helped = expectedHelpedUser;
            expectedCreatedModel.tutor = null;
            expectedCreatedModel.course = expectedCourse;

            AssertFunctions.assertValidRenderedViewForName(controller.create(validViewModel), "Create");

            this.appContext.getRepository().Received().add(Arg.Is<HelpRequest>(x => 
                    x.comment == expectedCreatedModel.comment &&
                    x.course == expectedCourse && 
                    x.helped == expectedHelpedUser && 
                    x.misunderstoodNotions == expectedCreatedModel.misunderstoodNotions && 
                    x.scheduleJson == expectedCreatedModel.scheduleJson && 
                    x.tutor == null
           )); // validate that the repository called Add method with the specified model
        }
    } 
}