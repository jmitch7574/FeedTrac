// IdentityController.cs
// This file is a modified / rewritten version of the original IdentityApiEndpointRouteBuilderExtensions
// https://github.com/dotnet/aspnetcore/blob/main/src/Identity/Core/src/IdentityApiEndpointRouteBuilderExtensions.cs
// Last Modified By: Jake Mitchell

using System.Text;
using System.Text.Encodings.Web;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.WebUtilities;
using FeedTrac.Server.Database;
using FeedTrac.Server.Extensions;
using Microsoft.AspNetCore.Identity.Data;
using FeedTrac.Server.Models.Forms;
using FeedTrac.Server.Models.Responses.Identity;
using FeedTrac.Server.Services;
using OtpNet;

namespace FeedTrac.Server.Controllers;

/// <summary>
/// A set of endpoints for managing user sign-ins and sign-ups
/// </summary>
[ApiController]
[Route("identity")]
public class IdentityController : ControllerBase
{
    private readonly FeedTracUserManager _userManager;
    private readonly SignInManager<ApplicationUser> _signInManager;
    private readonly EmailService _emailService;

    /// <summary>
    /// 
    /// </summary>
    /// <param name="userManager"></param>
    /// <param name="signInManager"></param>
    /// <param name="emailService"></param>
    public IdentityController(FeedTracUserManager userManager,
                              SignInManager<ApplicationUser> signInManager,
                              EmailService emailService)
    {
        _userManager = userManager;
        _signInManager = signInManager;
        _emailService = emailService;
    }

    /// <summary>
    /// API Endpoint for registring a student
    /// </summary>
    /// <param name="request">Body request containing the Registration information <see cref="RegisterUserRequest"/></param>
    /// <response code="200">User is successfully registered</response>
    /// <response code="400">User could not be created with the credentials supplied</response>
    [HttpPost("student/register")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> RegisterStudent([FromBody] RegisterUserRequest request)
    {
        var user = new ApplicationUser { UserName = request.Email, Email = request.Email, FirstName = request.FirstName, LastName = request.LastName };
        var result = await _userManager.CreateAsync(user, request.Password);

        if (!result.Succeeded)
        {
            return BadRequest(result.Errors);
        }
        
        await _userManager.AddToRoleAsync(user, "Student");

        return Ok();
    }

    /// <summary>
    /// Endpoint for registering a teacher
    /// </summary>
    /// <param name="request">Body request containing the Registration information <see cref="RegisterUserRequest"/></param>
    /// <response code="200">User created successfully</response>
    /// <response code="400">User could not be created with the credentials supplied</response>
    [HttpPost("teacher/register")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> RegisterTeacher([FromBody] RegisterUserRequest request)
    {
        await _userManager.RequireUser("Admin");
        var user = new ApplicationUser { UserName = request.Email, Email = request.Email, FirstName = request.FirstName, LastName = request.LastName };
        var result = await _userManager.CreateAsync(user, request.Password);

        if (!result.Succeeded)
        {
            return BadRequest(result.Errors);
        }

        // Enable two-factor authentication
        user.TwoFactorEnabled = true;

        byte[] secretKey = KeyGeneration.GenerateRandomKey(20); // 20 bytes (160 bits)
        string key = Base32Encoding.ToString(secretKey);

        user.TwoFactorSecret = key;

        // Add user to the Teacher role (was incorrectly adding to Student role)
        await _userManager.AddToRoleAsync(user, "Teacher");

        // Update the user with the changes
        await _userManager.UpdateAsync(user);
        
        await _emailService.TeacherWelcomeEmail(user, request.Password);
        
        RegisteredTeacher response = new RegisteredTeacher
        {
            TwoFactorKey = key
        };
        return Ok(response);
    }

    /// <summary>
    /// API Endpoint for logging in a student
    /// </summary>
    /// <param name="login"></param>
    /// <response code="200">Login was successful</response>
    /// <response code="400">User could not login with provided credentials</response>
    [HttpPost("student/login")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> StudentLogin([FromBody] StudentLoginRequest login)
    {

        var user = await _userManager.FindByEmailAsync(login.Email);
        if (user == null) return Unauthorized("Invalid credentials");

        // Check user role before allowing login
        var roles = await _userManager.GetRolesAsync(user);
        if (!roles.Contains("Student")) // Change condition as needed
        {
            return BadRequest("This endpoint is for student accounts only");
        }

        var result = await _signInManager.CheckPasswordSignInAsync(user, login.Password, false);
        if (!result.Succeeded) return Unauthorized("Invalid credentials");

        await _signInManager.SignInAsync(user, isPersistent: login.RememberMe);
        return Ok();
    }

    /// <summary>
    /// API Endpoint for logging in a teacher
    /// </summary>
    /// <param name="login"></param>
    /// <response code="200">Login successful</response>
    /// <response code="400">User could not be logged in with provided credentials</response>
    [HttpPost("teacher/login")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> TeacherLogin([FromBody] TeacherLoginRequest login)
    {
        var user = await _userManager.FindByEmailAsync(login.Email);
        if (user == null) return Unauthorized("User not found");

        // Check user role before allowing login
        var roles = await _userManager.GetRolesAsync(user);
        if (!roles.Contains("Teacher") && !roles.Contains("Admin")) // Change condition as needed
        {
            return BadRequest("Not a teacher account");
        }


        var isValid = user.Confirm2FaToken(login.TwoFactorCode);

        if (!isValid)
        {
            return Unauthorized("Invalid two-factor code");
        }


        var result = await _signInManager.CheckPasswordSignInAsync(user, login.Password, false);
        if (!result.Succeeded) return Unauthorized("Invalid credentials");

        await _signInManager.SignInAsync(user, isPersistent: login.RememberMe);
        return Ok();
    }

    /// <summary>
    /// Endpoint for initiation a forgotten password reset
    /// </summary>
    /// <param name="request">Forgot password request containing the users email</param>
    /// <response code="200">Successful sent forgot password email</response>
    /// <response code="404">Account with email not found</response>
    [HttpPost("forgotPassword/request")]
    public async Task<IActionResult> ForgotPassword([FromBody] ForgotPasswordRequest request)
    {
        var user = await _userManager.FindByEmailAsync(request.Email);
        if (user != null)
        {
            var code = await _userManager.GeneratePasswordResetTokenAsync(user);
            code = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(code));
            await _emailService.ForgotPassword(request.Email, HtmlEncoder.Default.Encode(code));
            return Ok();
        }

        throw new ResourceNotFoundException();
    }

    /// <summary>
    /// Endpoint for resetting password using a code generated by <see cref="ForgotPassword"/>
    /// </summary>
    /// <param name="request">Forgot Password followup containing the user's email, reset code and new password</param>
    /// <response code="200">Successfully updated password</response>
    /// <response code="400">Could not complete request</response>
    [HttpPost("forgotPassword/followup")]
    public async Task<IActionResult> ForgotPasswordFollowUp([FromBody] ResetPasswordRequest request)
    {
        var user = await _userManager.FindByEmailAsync(request.Email);
        if (user == null)
        {
            return BadRequest("Could not find user.");
        }
        var code = Encoding.UTF8.GetString(WebEncoders.Base64UrlDecode(request.ResetCode));
        var result = await _userManager.ResetPasswordAsync(user, code, request.NewPassword);
        if (!result.Succeeded)
        {
            return BadRequest(result.Errors);
        }
        return Ok();
    }

    /// <summary>
    /// Endpoint for resetting the user's password
    /// </summary>
    /// <param name="request">Reset password request containing the user's email, reset code, and new password</param>
    /// <response code="200">Successful reset password</response>
    /// <response code="400">Invalid Credentials</response>
    [HttpPost("resetPassword")]
    public async Task<IActionResult> ResetPassword([FromBody] FeedTracResetPasswordRequest request)
    {
        var user = await _userManager.RequireUser();
            
        var result = await _userManager.ChangePasswordAsync(user, request.CurrentPassword, request.NewPassword);
        if (!result.Succeeded)
        {
            return BadRequest(result.Errors);
        }
        return Ok();
    }

    /// <summary>
    /// Logout endpoint
    /// </summary>
    /// <response code="200">Logout successful</response>
    [HttpPost("logout")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<IActionResult> Logout()
    {
        await _signInManager.SignOutAsync();
        return Ok("Logged out successfully");
    }

    /// <summary>
    /// Authentication Endpoint
    /// </summary>
    /// <response code="200">Returns auth info</response>
    [HttpGet]
    [ProducesResponseType(typeof(AuthSummaryResponse), StatusCodes.Status200OK)]
    public async Task<IActionResult> IsAuthenticated()
    {
        if (User.Identity != null && User.Identity.IsAuthenticated)
        {
            var user = await _userManager.RequireUser();
            
            AuthSummaryResponse.AuthStatus status = AuthSummaryResponse.AuthStatus.AuthenticatedStudent;
            if (User.IsInRole("Teacher"))
            {
                status = AuthSummaryResponse.AuthStatus.AuthenticatedTeacher;
            }
            else if (User.IsInRole("Admin"))
            {
                status = AuthSummaryResponse.AuthStatus.AuthenticatedAdmin;
            }

            return Ok(new AuthSummaryResponse() { Status = status, UserInfo = new (user) });
        }

        return Ok(new AuthSummaryResponse() { Status = AuthSummaryResponse.AuthStatus.NotAuthenticated});

    }
}

