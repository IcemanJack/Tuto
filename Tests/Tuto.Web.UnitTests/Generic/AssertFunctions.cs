using System;
using System.Web.Mvc;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MvcContrib.TestHelper;

namespace Tuto.Web.UnitTests.Generic
{
    public static class AssertFunctions
    {
        public static void assertValidRenderedViewForName(ActionResult viewResult, String viewTitle)
        {
            Assert.IsNotNull(viewResult);
            viewResult.AssertViewRendered().ForView(viewTitle);
        }
    }
}
