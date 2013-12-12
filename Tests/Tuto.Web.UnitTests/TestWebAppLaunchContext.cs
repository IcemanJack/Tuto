using System.Linq;
using System.Web;
using NSubstitute;
using Ploeh.AutoFixture;
using Tuto.DataLayer.Models.Users;
using Tuto.DataLayer.Repository;
using Tuto.TestUtility.AutoFixture;
using Tuto.Web.Config;

namespace Tuto.Web.UnitTests
{
    //
    // Specific LaunchContext mainly used to simplify the unit tests
    //
    public class TestWebAppLaunchContext : WebAppLaunchContext
    {
        public TestWebAppLaunchContext() : base(getMockedRepository(), Substitute.For<HttpContextBase>())
        { }

        private static IEntityRepository getMockedRepository()
        {
            var fixture = new Fixture();
            fixture.Customizations.Add(new VirtualMembersOmitter());
            
            var mockedRepo = Substitute.For<IEntityRepository>();

            var mainManagers = fixture.CreateMany<Manager>(1).ToArray() as Manager[];
            var returnedMainManager = mainManagers.First();

            returnedMainManager.mail = WebAppConfiguration.MAIN_MANAGER_EMAIL;

            // add the main manager
            mockedRepo.getAll<Manager>().ReturnsForAnyArgs(mainManagers.AsQueryable());

            return mockedRepo;
        }
    }
}