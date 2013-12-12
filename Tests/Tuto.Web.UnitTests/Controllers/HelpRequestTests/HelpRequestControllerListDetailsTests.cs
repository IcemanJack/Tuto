using System.Web.Mvc;
using AutoMapper;
using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MvcContrib.TestHelper;
using NSubstitute;
using Ploeh.AutoFixture;
using Tuto.DataLayer.Models;
using Tuto.Web.ViewModels;

namespace Tuto.Web.UnitTests.Controllers.HelpRequestTests
{
    [TestClass]
    public class HelpRequestControllerListDetailsTests : HelpRequestControllerBaseTests
    {
        [TestMethod]
        public void details_should_return_view_with_viewModel_when_id_is_valid()
        {
            //Arrange
            var listDetails = this.fixture.Create<HelpRequest>();
            this.appContext.getRepository().getById<HelpRequest>(listDetails.id).Returns(listDetails);

            var viewModelExpected = Mapper.Map<HelpRequestListDetailsViewModel>(listDetails);

            //Action
            var viewResult = this.controller.details(listDetails.id) as ViewResult;
            var viewModelObtained = viewResult.ViewData.Model as HelpRequestListDetailsViewModel;

            //Assert 
            viewModelObtained.ShouldHave().AllProperties().EqualTo(viewModelExpected); // Utilise le package "fluent assertion"
        }

        [TestMethod]
        public void details_should_return_http_not_found_when_id_is_not_valid()
        {
            //Act
            var actionResult = this.controller.details(999999999);

            //Assert
            actionResult.AssertResultIs<HttpNotFoundResult>();
        }
    }
}
