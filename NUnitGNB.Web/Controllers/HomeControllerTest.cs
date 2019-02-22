using GNB.Web.Controllers;
using Microsoft.AspNetCore.Mvc;
using NUnit.Framework;

namespace GNB.Web.Tests.Controllers
{
    [TestFixture]
    internal class HomeControllerTest
    {
        private HomeController controller;

        [SetUp]
        public void SetUp() => controller = new HomeController();

        [Test]
        public void IndexTest()
        {
            IActionResult result = controller.Index();
            Assert.IsInstanceOf(typeof(ViewResult), result);
        }
    }
}
