using System.Web;
using System.Web.Mvc;
using AutoMapper;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MvcContrib.TestHelper;
using NSubstitute;
using Ploeh.AutoFixture;
using Tuto.DataLayer.Models;
using Tuto.DataLayer.Repository;
using Tuto.Web.UnitTests.Generic;
using Tuto.Web.ViewModels;

using Arg = NSubstitute.Arg;

namespace Tuto.Web.UnitTests.Controllers.HomeTests
{
    [TestClass]
    public class HomeControllerTests : HomeControllerBaseTests
    {
        [TestMethod]
        public void index_action_should_return_index_view()
        {
            AssertFunctions.assertValidRenderedViewForName(controller.index(), "Index");
        }
    }
}
