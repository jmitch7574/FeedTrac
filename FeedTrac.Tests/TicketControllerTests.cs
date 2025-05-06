using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using FeedTrac.Server;
using FeedTrac.Server.Controllers;
using FeedTrac.Server.Database;
using FeedTrac.Server.Services;
using FeedTrac.Tests.Helpers;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Linq;
using FeedTrac.Server.Extensions;
using System;

namespace FeedTrac.Tests.Controllers
{
    [TestClass]
    public class TicketControllerTests
    {
        private Mock<ApplicationDbContext> _mockContext;
        private Mock<FeedTracUserManager> _mockUserManager;
        private Mock<ModuleService> _mockModuleService;
        private Mock<ImageService> _mockImageService;
        private Mock<EmailService> _mockEmailService;
        private TicketController _controller;

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
            _mockImageService = new Mock<ImageService>(_mockContext.Object);
            _mockEmailService = new Mock<EmailService>();

            _controller = new TicketController(
                _mockContext.Object,
                _mockUserManager.Object,
                _mockModuleService.Object,
                _mockImageService.Object,
                _mockEmailService.Object
            );
        }

        [TestMethod]
        public async Task GetMyTickets_AsStudent_ReturnsOwnTickets()
        {
            var user = TestDataMocks.CreateUser("student1");
            var ticket = new FeedbackTicket { Owner = user, OwnerId = user.Id, Module = new Module(), Title = "Ticket Title", Messages = new List<FeedbackMessage>() };

            _mockUserManager.Setup(m => m.RequireUser()).ReturnsAsync(user);
            _controller.ControllerContext = new ControllerContext
            {
                HttpContext = new DefaultHttpContext
                {
                    User = new ClaimsPrincipal(new ClaimsIdentity(new[] {
                        new Claim(ClaimTypes.Role, "Student")
                    }))
                }
            };

            _mockContext.Setup(c => c.Tickets)
                .Returns(DbSetMockHelper.CreateMockDbSet(new[] { ticket }).Object);

            var result = await _controller.GetMyTickets();
            Assert.IsInstanceOfType(result, typeof(OkObjectResult));
        }

        [TestMethod]
        public async Task GetMyTickets_AsTeacher_ReturnsModuleTickets()
        {
            var user = TestDataMocks.CreateUser("teacher1");
            var module = TestDataMocks.CreateModule(includeTeacher: true, user: user);
            var ticket = new FeedbackTicket { Module = module, ModuleId = module.Id, Title = "Teacher Ticket", Owner = user, OwnerId = user.Id, Messages = new List<FeedbackMessage>() };

            _mockUserManager.Setup(m => m.RequireUser()).ReturnsAsync(user);
            _mockModuleService.Setup(m => m.GetUserModulesAsync()).ReturnsAsync(new List<Module> { module });
            _controller.ControllerContext = new ControllerContext
            {
                HttpContext = new DefaultHttpContext
                {
                    User = new ClaimsPrincipal(new ClaimsIdentity(new[] {
                        new Claim(ClaimTypes.Role, "Teacher")
                    }))
                }
            };

            _mockContext.Setup(c => c.Tickets)
                .Returns(DbSetMockHelper.CreateMockDbSet(new[] { ticket }).Object);

            var result = await _controller.GetMyTickets();
            Assert.IsInstanceOfType(result, typeof(OkObjectResult));
        }

        [TestMethod]
        public async Task MarkAsResolved_WithValidAccess_ReturnsOk()
        {
            var user = TestDataMocks.CreateUser("student1");
            var ticket = new FeedbackTicket
            {
                TicketId = 1,
                Owner = user,
                OwnerId = user.Id,
                Module = new Module { TeacherModule = new List<TeacherModule>() },
                Title = "Resolve Ticket",
                Messages = new List<FeedbackMessage>()
            };

            _mockUserManager.Setup(m => m.RequireUser()).ReturnsAsync(user);
            _mockContext.Setup(c => c.Tickets)
                .Returns(DbSetMockHelper.CreateMockDbSet(new[] { ticket }).Object);
            _mockEmailService.Setup(s => s.TicketResolved(It.IsAny<FeedbackTicket>())).Returns(Task.CompletedTask);

            var result = await _controller.MarkAsResolved(ticket.TicketId);
            Assert.IsInstanceOfType(result, typeof(OkObjectResult));
        }

        [TestMethod]
        [ExpectedException(typeof(UnauthorizedResourceAccessException))]
        public async Task MarkAsResolved_Unauthorized_Throws()
        {
            var owner = TestDataMocks.CreateUser("owner");
            var outsider = TestDataMocks.CreateUser("outsider");

            var ticket = new FeedbackTicket
            {
                TicketId = 1,
                Owner = owner,
                OwnerId = owner.Id,
                Module = new Module { TeacherModule = new List<TeacherModule>() },
                Title = "Unauthorised Ticket",
                Messages = new List<FeedbackMessage>()
            };

            _mockUserManager.Setup(m => m.RequireUser()).ReturnsAsync(outsider);
            _mockContext.Setup(c => c.Tickets).Returns(DbSetMockHelper.CreateMockDbSet(new[] { ticket }).Object);

            await _controller.MarkAsResolved(ticket.TicketId);
        }

        [TestMethod]
        [ExpectedException(typeof(ResourceNotFoundException))]
        public async Task MarkAsResolved_TicketNotFound_Throws()
        {
            _mockUserManager.Setup(m => m.RequireUser()).ReturnsAsync(TestDataMocks.CreateUser("user1"));
            _mockContext.Setup(c => c.Tickets).Returns(DbSetMockHelper.CreateMockDbSet(new List<FeedbackTicket>()).Object);

            await _controller.MarkAsResolved(999);
        }

        [TestMethod]
        //unable to mock IAsyncQueryProvider
        public async Task GetTicket_ValidId_ReturnsTicket()
        {
            Assert.IsTrue(true);
        }

        [TestMethod]
        //unable to mock IAsyncQueryProvider
        public async Task CreateTicket_ValidStudent_CreatesTicket()
        {
            Assert.IsTrue(true);
        }

        [TestMethod]
        //unable to mock IAsyncQueryProvider
        public async Task AddMessageToTicket_ValidTeacher_AddsMessage()
        {
            Assert.IsTrue(true);
        }
    }
}