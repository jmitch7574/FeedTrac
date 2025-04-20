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
using Microsoft.AspNetCore.Authorization;
using FeedTrac.Server.Models.Responses.Identity;
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
    private readonly IEmailSender<ApplicationUser> _emailSender;

    /// <summary>
    /// 
    /// </summary>
    /// <param name="userManager"></param>
    /// <param name="signInManager"></param>
    /// <param name="emailSender"></param>
    public IdentityController(FeedTracUserManager userManager,
                              SignInManager<ApplicationUser> signInManager,
                              IEmailSender<ApplicationUser> emailSender)
    {
        _userManager = userManager;
        _signInManager = signInManager;
        _emailSender = emailSender;
    }

    /// <summary>
    /// API Endpoint for registring a student
    /// </summary>
    /// <param name="request">Body request containing the Registration information <see cref="RegisterUserRequest"/></param>
    /// <returns>
    ///     <b>200:</b> Tjh User Successfully registered<br/>
    ///     <b>400:</b> Failed to register user <br/>
    /// </returns>
    [HttpPost("student/register")]
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
    /// <returns>
    ///     <b>200:</b> The User Successfully registered<br/>
    ///     <b>400:</b> Failed to register user <br/>
    /// </returns>
    [Authorize(Roles = "Admin")]
    [HttpPost("teacher/register")]
    public async Task<IActionResult> RegisterTeacher([FromBody] RegisterUserRequest request)
    {
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
    /// <returns></returns>
    [HttpPost("student/login")]
    public async Task<IActionResult> StudentLogin([FromBody] StudentLoginRequest login)
    {

        var user = await _userManager.FindByEmailAsync(login.Email);
        if (user == null) return Unauthorized("User not found");

        // Check user role before allowing login
        var roles = await _userManager.GetRolesAsync(user);
        if (!roles.Contains("Student")) // Change condition as needed
        {
            return Forbid("This endpoint is for student accounts only");
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
    /// <returns></returns>
    [HttpPost("teacher/login")]
    public async Task<IActionResult> TeacherLogin([FromBody] TeacherLoginRequest login)
    {
        var user = await _userManager.FindByEmailAsync(login.Email);
        if (user == null) return Unauthorized("User not found");

        // Check user role before allowing login
        var roles = await _userManager.GetRolesAsync(user);
        if (!roles.Contains("Teacher") && !roles.Contains("Admin")) // Change condition as needed
        {
            return Unauthorized("Not a teacher account");
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
    /// Forgot password endpoint - not implemented currently
    /// </summary>
    /// <param name="request">Forgot password request containing the users email</param>
    /// <returns></returns>
    [HttpPost("forgotPassword")]
    public async Task<IActionResult> ForgotPassword([FromBody] ForgotPasswordRequest request)
    {
        var user = await _userManager.FindByEmailAsync(request.Email);
        if (user != null && await _userManager.IsEmailConfirmedAsync(user))
        {
            var code = await _userManager.GeneratePasswordResetTokenAsync(user);
            code = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(code));
            await _emailSender.SendPasswordResetCodeAsync(user, request.Email, HtmlEncoder.Default.Encode(code));
        }
        return Ok();
    }

    /// <summary>
    /// Endpoint for resetting the user's password
    /// </summary>
    /// <param name="request">Reset password request containing the user's email, reset code, and new password</param>
    /// <returns></returns>
    [HttpPost("resetPassword")]
    public async Task<IActionResult> ResetPassword([FromBody] ResetPasswordRequest request)
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
    /// Logout endpoint
    /// </summary>
    [HttpPost("logout")]
    [ProducesResponseType(200)]
    public async Task<IActionResult> Logout()
    {
        await _signInManager.SignOutAsync();
        return Ok("Logged out successfully");
    }

    /// <summary>
    /// Authentication Endpoint
    /// </summary>
    /// <returns></returns>
    [HttpGet]
    [ProducesResponseType(typeof(AuthSummaryResponse), 200)]
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

