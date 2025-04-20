// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

// MODIFIED BY JAKE MITCHELL FOR THE PURPOSES OF THE FEEDTRAC PROJECT
// WOULD HAVE LOVED TO JUST DO AN INHERITANCE THING BUT THE IDENTITY API IS SEALED

namespace FeedTrac.Server.Models.Forms;

/// <summary>
/// Request type for endpoint <see cref="FeedTrac.Server.Controllers.IdentityController.TeacherLogin"/>
/// </summary>
public class TeacherLoginRequest
{
    /// <summary>
    /// The user's email address
    /// </summary>
    public required string Email { get; init; }

    /// <summary>
    /// The user's password
    /// </summary>
    public required string Password { get; init; }

    /// <summary>
    /// The Two factor code used to validate the sign in request
    /// </summary>
    public required string TwoFactorCode { get; init; }

    /// <summary>
    /// An optional parameter to indicate if the user's session should be remembered across browser restarts.
    /// </summary>
    public bool RememberMe { get; init; } = false;
}
