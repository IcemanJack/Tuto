using Microsoft.VisualStudio.TestTools.UnitTesting;
using MvcContrib.TestHelper;
using Arg = NSubstitute.Arg;

namespace Tuto.Web.UnitTests.Controllers.HomeTests
{
    [TestClass]
    public class HomeControllerTests : HomeControllerBaseTests
    {
        [TestMethod]
        public void index_action_should_return_index_view()
        {
            var returnedResult = controller.index();
            Assert.IsNotNull(returnedResult);
            returnedResult.AssertViewRendered().ForView("Index");
        }
    }
}
