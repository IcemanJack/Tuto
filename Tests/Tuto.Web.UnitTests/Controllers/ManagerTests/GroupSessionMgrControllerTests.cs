using Microsoft.VisualStudio.TestTools.UnitTesting;
using Tuto.Web.Controllers.Manager;

namespace Tuto.Web.UnitTests.Controllers.ManagerTests
{
    [TestClass]
    public class GroupSessionMgrControllerTests : ManagerComponentsBaseTest
    {
        private GroupSessionMgrController controller;

        [TestInitialize]
        public void groupSessionMgrTestsInit()
        {
            this.controller = new GroupSessionMgrController(this.appContext);
        }
    }
}
