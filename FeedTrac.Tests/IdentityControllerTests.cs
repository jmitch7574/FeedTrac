#nullable disable //suppress null warnings

using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using FeedTrac.Server.Controllers;
using FeedTrac.Server.Extensions;
using FeedTrac.Server.Services;
using Microsoft.AspNetCore.Identity;
using FeedTrac.Server.Database;    //for ApplicationUser
using Microsoft.AspNetCore.Identity.Data; //for RegisterUserRequest
using FeedTrac.Server.Models.Forms; //for StudentLoginRequest
using FeedTrac.Server.Models.Responses.Identity; //for RegisteredTeacher
using System.Text; //for simulating base64 encoding in password reset token
using Microsoft.AspNetCore.Http;
using System.Security.Claims; //for simulating authenticated user
using Microsoft.Extensions.Options;
using Microsoft.AspNetCore.Authentication.BearerToken;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

[TestClass]
public class IdentityControllerTests
{
    private Mock<FeedTracUserManager> _userManagerMock;
    private Mock<SignInManager<ApplicationUser>> _signInManagerMock;
    private Mock<EmailService> _emailServiceMock;
    private Mock<IOptionsMonitor<BearerTokenOptions>> _optionsMock;

    [TestInitialize]
    public void Setup()
    {
        var store = new Mock<IUserStore<ApplicationUser>>();
        _userManagerMock = new Mock<FeedTracUserManager>(store.Object, null, null, null, null, null, null, null, null, new Mock<IHttpContextAccessor>().Object);

        var context = new Mock<Microsoft.AspNetCore.Http.HttpContext>();
        _signInManagerMock = new Mock<SignInManager<ApplicationUser>>(
            _userManagerMock.Object,
            new Mock<Microsoft.AspNetCore.Http.IHttpContextAccessor>().Object,
            new Mock<IUserClaimsPrincipalFactory<ApplicationUser>>().Object,
            null, null, null, null
        );

        _emailServiceMock = new Mock<EmailService>();
        _optionsMock = new Mock<IOptionsMonitor<BearerTokenOptions>>();
    }

    [TestMethod]
    public async Task RegisterStudent_ReturnsOk_WhenSuccessful()
    {
        //Arrange
        var request = new RegisterUserRequest
        {
            Email = "test@example.com",
            Password = "Test123!",
            FirstName = "Test",
            LastName = "User"
        };

        _userManagerMock.Setup(x => x.CreateAsync(It.IsAny<ApplicationUser>(), It.IsAny<string>()))
                        .ReturnsAsync(IdentityResult.Success);

        _userManagerMock.Setup(x => x.AddToRoleAsync(It.IsAny<ApplicationUser>(), "Student"))
                        .ReturnsAsync(IdentityResult.Success);

        var controller = new IdentityController(
            _userManagerMock.Object,
            _signInManagerMock.Object,
            _emailServiceMock.Object
        );

        //Act
        var result = await controller.RegisterStudent(request);

        //Assert
        Assert.IsInstanceOfType(result, typeof(OkResult));
    }
    [TestMethod]
    public async Task RegisterStudent_ReturnsBadRequest_WhenPasswordIsInvalid()
    {
        //Arrange: create fake registration with weak password
        var request = new RegisterUserRequest
        {
            Email = "badpass@example.com",
            Password = "123",  //intentionally weak as per ASP.NET rules, triggers password failure
            FirstName = "Jane",
            LastName = "Doe"
        };

        //mock UserManager to simulate failed registration due to password policy
        _userManagerMock.Setup(x => x.CreateAsync(It.IsAny<ApplicationUser>(), It.IsAny<string>()))
                .ReturnsAsync(IdentityResult.Failed(new IdentityError
                {
                    Code = "PasswordTooWeak",
                    Description = "Password too weak"
                }));

        //mocked dependencies
        var controller = new IdentityController(
            _userManagerMock.Object,
            _signInManagerMock.Object,
            _emailServiceMock.Object
        );

        //Act: attempt to register student using weak password
        var result = await controller.RegisterStudent(request);

        //Assert: response should be a BadRequest (HTTP 400)
        Assert.IsInstanceOfType(result, typeof(BadRequestObjectResult));

        var badRequest = result as BadRequestObjectResult;
        var errors = badRequest!.Value as IEnumerable<IdentityError>;

        Assert.IsNotNull(errors); //object is not null
        Assert.IsTrue(errors.Any(e => e.Description.Contains("Password too weak"))); //confirm expected error message
    }
    [TestMethod]
    public async Task StudentLogin_ReturnsUnauthorized_WhenPasswordIsIncorrect()
    {
        //Arrange: simulate a valid user and correct role
        var user = new ApplicationUser
        {
            Email = "student@example.com",
            UserName = "student@example.com"
        };

        var loginRequest = new StudentLoginRequest
        {
            Email = "student@example.com",
            Password = "wrong-password",
            RememberMe = false
        };

        //user exists
        _userManagerMock.Setup(x => x.FindByEmailAsync(loginRequest.Email))
            .ReturnsAsync(user);

        //user is assigned the "Student" role
        _userManagerMock.Setup(x => x.GetRolesAsync(user))
            .ReturnsAsync(new List<string> { "Student" });

        //password is incorrect
        _signInManagerMock.Setup(x => x.CheckPasswordSignInAsync(user, loginRequest.Password, false))
            .ReturnsAsync(Microsoft.AspNetCore.Identity.SignInResult.Failed);

        var controller = new IdentityController(
            _userManagerMock.Object,
            _signInManagerMock.Object,
            _emailServiceMock.Object
        );

        var result = await controller.StudentLogin(loginRequest); //try to log in with wrong password

        Assert.IsInstanceOfType(result, typeof(UnauthorizedObjectResult)); //should return "401 Unauthorized"
    }
    [TestMethod]
    public async Task StudentLogin_ReturnsOk_WhenCredentialsAreValid()
    {
        //set up a known student user
        var user = new ApplicationUser
        {
            Email = "student@example.com",
            UserName = "student@example.com"
        };

        var loginRequest = new StudentLoginRequest
        {
            Email = "student@example.com",
            Password = "CorrectPassword123!",
            RememberMe = true
        };

        //user is found by email
        _userManagerMock.Setup(x => x.FindByEmailAsync(loginRequest.Email))
            .ReturnsAsync(user);

        //user is in the "Student" role
        _userManagerMock.Setup(x => x.GetRolesAsync(user))
            .ReturnsAsync(new List<string> { "Student" });

        //password check passes
        _signInManagerMock.Setup(x => x.CheckPasswordSignInAsync(user, loginRequest.Password, false))
            .ReturnsAsync(Microsoft.AspNetCore.Identity.SignInResult.Success);

        //sign-in succeeds
        _signInManagerMock.Setup(x => x.SignInAsync(user, loginRequest.RememberMe, null))
            .Returns(Task.CompletedTask);

        var controller = new IdentityController(
            _userManagerMock.Object,
            _signInManagerMock.Object,
            _emailServiceMock.Object
        );

        //try to log in
        var result = await controller.StudentLogin(loginRequest);

        //expect HTTP 200 OK
        Assert.IsInstanceOfType(result, typeof(OkResult));
    }
    [TestMethod]
    public async Task RegisterStudent_ReturnsBadRequest_WhenEmailAlreadyExists()
    {
        //Arrange: registration request with a duplicate email
        var request = new RegisterUserRequest
        {
            Email = "existing@example.com",
            Password = "Test!123",
            FirstName = "Sam",
            LastName = "Smith"
        };

        //UserManager.CreateAsync fails due to existing email
        _userManagerMock.Setup(x => x.CreateAsync(It.IsAny<ApplicationUser>(), It.IsAny<string>()))
            .ReturnsAsync(IdentityResult.Failed(new IdentityError
            {
                Code = "DuplicateEmail",
                Description = "Email 'existing@example.com' is already taken."
            }));

        var controller = new IdentityController(
            _userManagerMock.Object,
            _signInManagerMock.Object,
            _emailServiceMock.Object
        );

        var result = await controller.RegisterStudent(request);

        Assert.IsInstanceOfType(result, typeof(BadRequestObjectResult));

        var badRequest = result as BadRequestObjectResult;
        var errors = badRequest!.Value as IEnumerable<IdentityError>;

        Assert.IsNotNull(errors);
        Assert.IsTrue(errors.Any(e => e.Description.Contains("already taken")));
    }
    [TestMethod]
    public async Task RegisterTeacher_ReturnsOk_With2FAKey()
    {
        //Arrange
        var request = new RegisterUserRequest
        {
            Email = "teacher@example.com",
            Password = "Secure!456",
            FirstName = "Alice",
            LastName = "Johnson"
        };

        var createdUser = new ApplicationUser
        {
            Email = request.Email,
            UserName = request.Email,
            FirstName = request.FirstName,
            LastName = request.LastName
        };

        _userManagerMock.Setup(x => x.CreateAsync(It.IsAny<ApplicationUser>(), It.IsAny<string>()))
            .Callback<ApplicationUser, string>((user, pwd) => {
                //set this on the user to simulate side effect of creation
                user.TwoFactorSecret = "FAKE2FAKEY";
            })
            .ReturnsAsync(IdentityResult.Success);

        _userManagerMock.Setup(x => x.AddToRoleAsync(It.IsAny<ApplicationUser>(), "Teacher"))
            .ReturnsAsync(IdentityResult.Success);

        _userManagerMock.Setup(x => x.UpdateAsync(It.IsAny<ApplicationUser>()))
            .ReturnsAsync(IdentityResult.Success);

        var controller = new IdentityController(
            _userManagerMock.Object,
            _signInManagerMock.Object,
            _emailServiceMock.Object
        );
        var result = await controller.RegisterTeacher(request);

        Assert.IsInstanceOfType(result, typeof(OkObjectResult));

        var okResult = result as OkObjectResult;
        Assert.IsNotNull(okResult?.Value);

        //validate that the response contains a 2FA key
        var response = okResult.Value as RegisteredTeacher;
        Assert.IsNotNull(response);
        Assert.IsFalse(string.IsNullOrWhiteSpace(response!.TwoFactorKey));
    }
    [TestMethod]
    public async Task RegisterTeacher_ReturnsBadRequest_WhenPasswordIsInvalid()
    {
        //Arrange: Build a teacher registration request with a weak password
        var request = new RegisterUserRequest
        {
            Email = "teacherfail@example.com",
            Password = "123",  //intentionally weak as per ASP.NET rules, triggers password failure
            FirstName = "Failed",
            LastName = "Teacher"
        };

        //UserManager.CreateAsync returns a failure due to password
        _userManagerMock.Setup(x => x.CreateAsync(It.IsAny<ApplicationUser>(), It.IsAny<string>()))
            .ReturnsAsync(IdentityResult.Failed(new IdentityError
            {
                Code = "PasswordTooWeak",
                Description = "Password does not meet complexity requirements."
            }));

        var controller = new IdentityController(
            _userManagerMock.Object,
            _signInManagerMock.Object,
            _emailServiceMock.Object
        );

        var result = await controller.RegisterTeacher(request);

        Assert.IsInstanceOfType(result, typeof(BadRequestObjectResult));

        var badRequest = result as BadRequestObjectResult;
        var errors = badRequest!.Value as IEnumerable<IdentityError>;

        Assert.IsNotNull(errors);
        Assert.IsTrue(errors.Any(e => e.Description.Contains("complexity requirements")));
    }
    [TestMethod]
    public async Task StudentLogin_ReturnsUnauthorized_WhenUserDoesNotExist()
    {
        //login attempt with a non-existent user
        var loginRequest = new StudentLoginRequest
        {
            Email = "ghost@example.com",
            Password = "DoesntMatter123!",
            RememberMe = false
        };

        //UserManager returns null for unknown email
        _userManagerMock.Setup(x => x.FindByEmailAsync(loginRequest.Email))
            .ReturnsAsync((ApplicationUser)null);

        var controller = new IdentityController(
            _userManagerMock.Object,
            _signInManagerMock.Object,
            _emailServiceMock.Object
        );

        var result = await controller.StudentLogin(loginRequest);

        //401 Unauthorized with "User not found" message
        Assert.IsInstanceOfType(result, typeof(UnauthorizedObjectResult));

        var unauthorized = result as UnauthorizedObjectResult;
        Assert.IsNotNull(unauthorized);
        Assert.AreEqual("User not found", unauthorized.Value);
    }
    [TestMethod]
    public async Task StudentLogin_ReturnsForbid_WhenUserIsNotStudent()
    {
        // Arrange: User exists, but has a non-student role
        var user = new ApplicationUser
        {
            Email = "teacherlogginginasstudent@example.com",
            UserName = "teacherlogginginasstudent@example.com"
        };

        var loginRequest = new StudentLoginRequest
        {
            Email = user.Email,
            Password = "ValidPass123!",
            RememberMe = false
        };

        //user is found
        _userManagerMock.Setup(x => x.FindByEmailAsync(loginRequest.Email))
            .ReturnsAsync(user);

        //user has a different role (e.g., Teacher)
        _userManagerMock.Setup(x => x.GetRolesAsync(user))
            .ReturnsAsync(new List<string> { "Teacher" });

        var controller = new IdentityController(
            _userManagerMock.Object,
            _signInManagerMock.Object,
            _emailServiceMock.Object
        );

        var result = await controller.StudentLogin(loginRequest);

        // Assert: ForbidResult with custom message
        Assert.IsInstanceOfType(result, typeof(ForbidResult));
    }
    [TestMethod]
    public async Task TeacherLogin_ReturnsUnauthorized_WhenUserIsNotTeacherOrAdmin()
    {
        //user who is not a teacher or admin
        var user = new ApplicationUser
        {
            Email = "studentposingasteacher@example.com",
            UserName = "studentposingasteacher@example.com",
            TwoFactorSecret = "FAKESECRET"
        };

        var loginRequest = new TeacherLoginRequest
        {
            Email = user.Email,
            Password = "CorrectPassword123!",
            TwoFactorCode = "123456",
            RememberMe = false
        };

        _userManagerMock.Setup(x => x.FindByEmailAsync(loginRequest.Email))
            .ReturnsAsync(user);

        //wrong role — user is not a teacher or admin
        _userManagerMock.Setup(x => x.GetRolesAsync(user))
            .ReturnsAsync(new List<string> { "Student" });

        //assume 2FA is valid
        _userManagerMock.Setup(x => x.GetAuthenticatorKeyAsync(user))
            .ReturnsAsync("FAKESECRET");

        //password is correct
        _signInManagerMock.Setup(x => x.CheckPasswordSignInAsync(user, loginRequest.Password, false))
            .ReturnsAsync(Microsoft.AspNetCore.Identity.SignInResult.Success);

        var controller = new IdentityController(
            _userManagerMock.Object,
            _signInManagerMock.Object,
            _emailServiceMock.Object
        );

        var result = await controller.TeacherLogin(loginRequest);

        Assert.IsInstanceOfType(result, typeof(UnauthorizedObjectResult));

        var unauthorized = result as UnauthorizedObjectResult;
        Assert.IsNotNull(unauthorized);
        Assert.AreEqual("Not a teacher account", unauthorized.Value);
    }
    [TestMethod]
    public async Task ForgotPassword_ReturnsOk_WhenUserExistsAndEmailConfirmed()
    {
        var user = new ApplicationUser
        {
            Email = "resetme@example.com",
            UserName = "resetme@example.com"
        };

        var request = new ForgotPasswordRequest
        {
            Email = user.Email
        };

        _userManagerMock.Setup(x => x.FindByEmailAsync(request.Email))
            .ReturnsAsync(user);

        _userManagerMock.Setup(x => x.IsEmailConfirmedAsync(user))
            .ReturnsAsync(true);

        _userManagerMock.Setup(x => x.GeneratePasswordResetTokenAsync(user))
            .ReturnsAsync("mock-reset-token");

        _emailServiceMock.Setup(x => x.ForgotPassword(
            request.Email,
            It.IsAny<string>()
        )).Returns(Task.CompletedTask);

        var controller = new IdentityController(
            _userManagerMock.Object,
            _signInManagerMock.Object,
            _emailServiceMock.Object
        );

        var result = await controller.ForgotPassword(request);

        Assert.IsInstanceOfType(result, typeof(OkResult));
    }
    [TestMethod]
    public async Task ForgotPassword_ReturnsOk_WhenUserDoesNotExistOrEmailNotConfirmed()
    {
        var request = new ForgotPasswordRequest
        {
            Email = "ghost@example.com"
        };

        //Case 1: User not found
        _userManagerMock.Setup(x => x.FindByEmailAsync(request.Email))
            .ReturnsAsync((ApplicationUser)null);

        var controller = new IdentityController(
            _userManagerMock.Object,
            _signInManagerMock.Object,
            _emailServiceMock.Object
        );

        var result1 = await controller.ForgotPassword(request);

        Assert.IsInstanceOfType(result1, typeof(OkResult));

        //Case 2: User found, but email not confirmed
        var unconfirmedUser = new ApplicationUser
        {
            Email = request.Email,
            UserName = request.Email
        };

        _userManagerMock.Setup(x => x.FindByEmailAsync(request.Email))
            .ReturnsAsync(unconfirmedUser);

        _userManagerMock.Setup(x => x.IsEmailConfirmedAsync(unconfirmedUser))
            .ReturnsAsync(false);

        var result2 = await controller.ForgotPassword(request);

        Assert.IsInstanceOfType(result2, typeof(OkResult));
    }
    [TestMethod]
    public async Task ResetPassword_ReturnsOk_WhenResetTokenIsValid()
    {
        //setup user and reset request
        var user = new ApplicationUser
        {
            Email = "resetme@example.com",
            UserName = "resetme@example.com"
        };

        var resetCode = Convert.ToBase64String(Encoding.UTF8.GetBytes("valid-token"));

        var request = new ResetPasswordRequest
        {
            Email = user.Email,
            ResetCode = resetCode,
            NewPassword = "NewSecurePassword!1"
        };

        //user is found
        _userManagerMock.Setup(x => x.FindByEmailAsync(request.Email))
            .ReturnsAsync(user);

        //reset succeeds
        _userManagerMock.Setup(x => x.ResetPasswordAsync(user, "valid-token", request.NewPassword))
            .ReturnsAsync(IdentityResult.Success);

        var controller = new IdentityController(
            _userManagerMock.Object,
            _signInManagerMock.Object,
            _emailServiceMock.Object
        );

        var result = await controller.ForgotPasswordFollowUp(request);

        Assert.IsInstanceOfType(result, typeof(OkResult));
    }
    [TestMethod]
    public async Task ResetPassword_ReturnsBadRequest_WhenResetTokenIsInvalid()
    {
        var user = new ApplicationUser
        {
            Email = "resetme@example.com",
            UserName = "resetme@example.com"
        };

        var resetCode = Convert.ToBase64String(Encoding.UTF8.GetBytes("invalid-token"));

        var request = new ResetPasswordRequest
        {
            Email = user.Email,
            ResetCode = resetCode,
            NewPassword = "AnotherNewPassword!2"
        };

        _userManagerMock.Setup(x => x.FindByEmailAsync(request.Email))
            .ReturnsAsync(user);

        _userManagerMock.Setup(x => x.ResetPasswordAsync(user, "invalid-token", request.NewPassword))
            .ReturnsAsync(IdentityResult.Failed(new IdentityError
            {
                Code = "InvalidToken",
                Description = "Reset token is invalid or expired."
            }));

        var controller = new IdentityController(
            _userManagerMock.Object,
            _signInManagerMock.Object,
            _emailServiceMock.Object
        );

        var result = await controller.ForgotPasswordFollowUp(request);

        Assert.IsInstanceOfType(result, typeof(BadRequestObjectResult));

        var badRequest = result as BadRequestObjectResult;
        var errors = badRequest!.Value as IEnumerable<IdentityError>;

        Assert.IsNotNull(errors);
        Assert.IsTrue(errors.Any(e => e.Description.Contains("invalid or expired")));
    }
    [TestMethod]
    public async Task ResetPassword_ReturnsBadRequest_WhenUserNotFound()
    {
        var request = new ResetPasswordRequest
        {
            Email = "ghost@example.com",
            ResetCode = Convert.ToBase64String(Encoding.UTF8.GetBytes("any-token")),
            NewPassword = "SomeNewPass123!"
        };

        _userManagerMock.Setup(x => x.FindByEmailAsync(request.Email))
            .ReturnsAsync((ApplicationUser)null);  //user not found

        var controller = new IdentityController(
            _userManagerMock.Object,
            _signInManagerMock.Object,
            _emailServiceMock.Object
        );

        var result = await controller.ForgotPasswordFollowUp(request);

        Assert.IsInstanceOfType(result, typeof(BadRequestObjectResult));

        var badRequest = result as BadRequestObjectResult;
        Assert.IsNotNull(badRequest);
        Assert.AreEqual("Could not find user.", badRequest.Value);
    }
    [TestMethod]
    public async Task Logout_ReturnsOk_WhenSuccessful()
    {
        //sign-out succeeds
        _signInManagerMock.Setup(x => x.SignOutAsync())
            .Returns(Task.CompletedTask);

        var controller = new IdentityController(
            _userManagerMock.Object,
            _signInManagerMock.Object,
            _emailServiceMock.Object
        );
        var result = await controller.Logout();

        //should return 200 OK with expected message
        Assert.IsInstanceOfType(result, typeof(OkObjectResult));

        var ok = result as OkObjectResult;
        Assert.IsNotNull(ok);
        Assert.AreEqual("Logged out successfully", ok.Value);
    }
    [TestMethod]
    public async Task IsAuthenticated_ReturnsNotAuthenticated_WhenUserIsNotSignedIn()
    {
        // Arrange: simulate unauthenticated user
        var httpContext = new DefaultHttpContext();
        httpContext.User = new ClaimsPrincipal(new ClaimsIdentity()); // no identity, not authenticated

        var controller = new IdentityController(
            _userManagerMock.Object,
            _signInManagerMock.Object,
            _emailServiceMock.Object
        );
        {
            controller.ControllerContext = new ControllerContext
            {
                HttpContext = httpContext
            };
        };

        var result = await controller.IsAuthenticated();

        Assert.IsInstanceOfType(result, typeof(OkObjectResult));

        var ok = result as OkObjectResult;
        Assert.IsNotNull(ok);

        var response = ok.Value as AuthSummaryResponse;
        Assert.IsNotNull(response);
        Assert.AreEqual(AuthSummaryResponse.AuthStatus.NotAuthenticated, response.Status);
    }
    [TestMethod]
    public async Task IsAuthenticated_ReturnsStudentStatus_WhenUserIsStudent()
    {
        var identity = new ClaimsIdentity(new[]
        {
            new Claim(ClaimTypes.Role, "Student")
        }, "TestAuth");

        var httpContext = new DefaultHttpContext
        {
            User = new ClaimsPrincipal(identity)
        };

        _userManagerMock.Setup(x => x.GetUserAsync(It.IsAny<ClaimsPrincipal>()))
            .ReturnsAsync(new ApplicationUser());

        var controller = new IdentityController(
            _userManagerMock.Object,
            _signInManagerMock.Object,
            _emailServiceMock.Object
        );
        {
            controller.ControllerContext = new ControllerContext
            {
                HttpContext = httpContext
            };
        };

        var result = await controller.IsAuthenticated();

        Assert.IsInstanceOfType(result, typeof(OkObjectResult));
        var ok = result as OkObjectResult;
        var response = ok!.Value as AuthSummaryResponse;
        Assert.AreEqual(AuthSummaryResponse.AuthStatus.AuthenticatedStudent, response!.Status);
    }
    [TestMethod]
    public async Task IsAuthenticated_ReturnsTeacherStatus_WhenUserIsTeacher()
    {
        var identity = new ClaimsIdentity(new[]
        {
            new Claim(ClaimTypes.Role, "Teacher")
        }, "TestAuth");

        var httpContext = new DefaultHttpContext
        {
            User = new ClaimsPrincipal(identity)
        };

        _userManagerMock.Setup(x => x.GetUserAsync(It.IsAny<ClaimsPrincipal>()))
            .ReturnsAsync(new ApplicationUser());

        var controller = new IdentityController(
            _userManagerMock.Object,
            _signInManagerMock.Object,
            _emailServiceMock.Object
        );
        {
            controller.ControllerContext = new ControllerContext
            {
                HttpContext = httpContext
            };
        };

        var result = await controller.IsAuthenticated();

        Assert.IsInstanceOfType(result, typeof(OkObjectResult));
        var ok = result as OkObjectResult;
        var response = ok!.Value as AuthSummaryResponse;
        Assert.AreEqual(AuthSummaryResponse.AuthStatus.AuthenticatedTeacher, response!.Status);
    }
    [TestMethod]
    public async Task IsAuthenticated_ReturnsAdminStatus_WhenUserIsAdmin()
    {
        var identity = new ClaimsIdentity(new[]
        {
            new Claim(ClaimTypes.Role, "Admin")
        }, "TestAuth");

        var httpContext = new DefaultHttpContext
        {
            User = new ClaimsPrincipal(identity)
        };

        _userManagerMock.Setup(x => x.GetUserAsync(It.IsAny<ClaimsPrincipal>()))
            .ReturnsAsync(new ApplicationUser());

        var controller = new IdentityController(
            _userManagerMock.Object,
            _signInManagerMock.Object,
            _emailServiceMock.Object
        );
        {
            controller.ControllerContext = new ControllerContext
            {
                HttpContext = httpContext
            };
        };

        var result = await controller.IsAuthenticated();

        Assert.IsInstanceOfType(result, typeof(OkObjectResult));
        var ok = result as OkObjectResult;
        var response = ok!.Value as AuthSummaryResponse;
        Assert.AreEqual(AuthSummaryResponse.AuthStatus.AuthenticatedAdmin, response!.Status);
    }
}