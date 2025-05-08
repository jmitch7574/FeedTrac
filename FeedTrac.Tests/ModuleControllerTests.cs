#nullable disable //suppress null warnings

using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using FeedTrac.Server;
using FeedTrac.Server.Controllers;
using FeedTrac.Server.Database;
using FeedTrac.Server.Extensions;
using FeedTrac.Server.Models.Responses.Modules;
using FeedTrac.Server.Services;
using FeedTrac.Tests.Helpers;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System;

namespace FeedTrac.Tests.Controllers
{
    [TestClass]
    public class ModuleControllerTests
    {
        private Mock<ApplicationDbContext> _mockContext;
        private Mock<FeedTracUserManager> _mockUserManager;
        private Mock<ModuleService> _mockModuleService;
        private ModuleController _controller;
        private Mock<PasswordGenerator> _mockPasswordGenerator;

        [TestInitialize]
        public void Setup()
        {
            _mockContext = new Mock<ApplicationDbContext>(new DbContextOptions<ApplicationDbContext>());

            _mockUserManager = new Mock<FeedTracUserManager>(
                Mock.Of<IUserStore<ApplicationUser>>(),
                Mock.Of<IOptions<IdentityOptions>>(),
                Mock.Of<IPasswordHasher<ApplicationUser>>(),
                new List<IUserValidator<ApplicationUser>>(),
                new List<IPasswordValidator<ApplicationUser>>(),
                Mock.Of<ILookupNormalizer>(),
                new IdentityErrorDescriber(),
                Mock.Of<IServiceProvider>(),
                Mock.Of<ILogger<UserManager<ApplicationUser>>>(),
                Mock.Of<IHttpContextAccessor>()
            );

            _mockModuleService = new Mock<ModuleService>(_mockContext.Object, _mockUserManager.Object);

            _controller = new ModuleController(_mockContext.Object, _mockUserManager.Object, _mockModuleService.Object);
            
            _mockPasswordGenerator = new Mock<PasswordGenerator>(_mockContext.Object);
        }

        [TestMethod]
        [ExpectedException(typeof(InsufficientRolesException))]
        public async Task GetAllModules_UnauthorizedUser_Throws()
        {
            var user = TestDataMocks.CreateUser("unauthorised");

            _mockUserManager.Setup(x => x.RequireUser("Teacher", "Admin")).ReturnsAsync(user);
            _mockModuleService.Setup(s => s.GetAllModulesAsync())
                            .ThrowsAsync(new InsufficientRolesException());

            await _controller.GetAllModules();
        }

        [TestMethod]
        //service is mocked and returns fake data, returns module collection with expected module count
        public async Task GetAllModules_AsAdmin_ReturnsModules()
        {
            var admin = TestDataMocks.CreateUser("admin-user");
            var testModules = new List<Module> { TestDataMocks.CreateModule() };

            _mockUserManager.Setup(m => m.RequireUser("Teacher", "Admin")).ReturnsAsync(admin);
            _mockModuleService.Setup(m => m.GetAllModulesAsync()).ReturnsAsync(testModules);

            var result = await _controller.GetAllModules();

            Assert.IsInstanceOfType(result, typeof(OkObjectResult));
            var ok = result as OkObjectResult;
            Assert.IsInstanceOfType(ok.Value, typeof(ModuleCollectionDto));
            var dto = ok.Value as ModuleCollectionDto;
            Assert.AreEqual(1, dto.Modules.Count());
        }

        [TestMethod]
        //confimrs controller accepts role, returns mock module. Result is wrapped in ModuleCollectionDto
        public async Task GetUserModules_ReturnsOkWithModules()
        {
            var user = TestDataMocks.CreateUser("test-user-id");
            var modules = new List<Module> { TestDataMocks.CreateModule(includeStudent: true, user: user) };

            _mockUserManager.Setup(m => m.RequireUser("Student", "Teacher")).ReturnsAsync(user);
            _mockModuleService.Setup(m => m.GetUserModulesAsync()).ReturnsAsync(modules);

            var result = await _controller.GetUserModules();

            Assert.IsInstanceOfType(result, typeof(OkObjectResult));
            var ok = result as OkObjectResult;
            Assert.IsInstanceOfType(ok.Value, typeof(ModuleCollectionDto));
            var dto = ok.Value as ModuleCollectionDto;
            Assert.AreEqual(1, dto.Modules.Count());
        }

        [TestMethod]
        //confirms admin authorisation, module lookup & delete.
        public async Task DeleteModule_ValidId_DeletesModule()
        {
            var admin = TestDataMocks.CreateUser("admin");
            var modules = new List<Module> { TestDataMocks.CreateModule() };

            _mockUserManager.Setup(m => m.RequireUser("Admin")).ReturnsAsync(admin);
            _mockContext.Setup(c => c.Modules).Returns(DbSetMockHelper.CreateMockDbSet(modules).Object);
            _mockContext.Setup(c => c.SaveChangesAsync(default)).ReturnsAsync(1);

            var result = await _controller.DeleteModule(modules[0].Id);

            Assert.IsInstanceOfType(result, typeof(OkResult));
            Assert.AreEqual(0, modules.Count); //proves Remove worked on the list
        }

        [TestMethod]
        [ExpectedException(typeof(ResourceNotFoundException))]
        public async Task DeleteModule_InvalidId_Throws()
        {
            _mockUserManager.Setup(m => m.RequireUser("Admin")).ReturnsAsync(TestDataMocks.CreateUser("admin"));
            _mockContext.Setup(c => c.Modules).Returns(DbSetMockHelper.CreateMockDbSet(new List<Module>()).Object);
            await _controller.DeleteModule(404);
        }

        [TestMethod]
        public async Task LeaveModule_ValidRequest_Success()
        {
            var user = TestDataMocks.CreateUser("student1");
            var module = TestDataMocks.CreateModule(includeStudent: true, user: user);

            _mockUserManager.Setup(m => m.RequireUser("Student")).ReturnsAsync(user);
            _mockContext.Setup(c => c.Modules).Returns(DbSetMockHelper.CreateMockDbSet(new[] { module }).Object);

            var result = await _controller.LeaveModule(module.Id);
            Assert.IsInstanceOfType(result, typeof(OkResult));
        }

        [TestMethod]
        [ExpectedException(typeof(UnauthorizedResourceAccessException))]
        public async Task LeaveModule_NotPartOfModule_Throws()
        {
            var user = TestDataMocks.CreateUser("user1");
            var module = TestDataMocks.CreateModule();

            _mockUserManager.Setup(m => m.RequireUser("Student")).ReturnsAsync(user);
            _mockContext.Setup(c => c.Modules).Returns(DbSetMockHelper.CreateMockDbSet(new[] { module }).Object);

            await _controller.LeaveModule(module.Id);
        }

        [TestMethod]
        public async Task CreateModule_AsTeacher_Success()
        {
            var teacher = TestDataMocks.CreateUser("Teacher");

            _mockUserManager.Setup(m => m.RequireUser("Teacher", "Admin")).ReturnsAsync(teacher);
            _mockContext.Setup(c => c.Modules).Returns(DbSetMockHelper.CreateMockDbSet(new List<Module>()).Object);

            var result = await _controller.CreateModule("New Module");
            var okResult = result as OkObjectResult;

            Assert.IsNotNull(okResult);
            Assert.IsInstanceOfType(okResult.Value, typeof(ModuleDto));
        }
    }
}